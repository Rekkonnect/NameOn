#nullable enable

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using NameOn.Core;
using NameOn.Core.Utilities;
using RoseLynn.Analyzers;
using System.Collections.Generic;
using System.Linq;

// The analyzer should not be run concurrently due to the state that it preserves
#pragma warning disable RS1026 // Enable concurrent execution

namespace NameOn
{
    using static Diagnostics;

    using NameOfRestrictionDictionary = SymbolDictionary<NameOfRestrictionAssociation>;
    using SymbolNameOfStateDictionary = SymbolDictionary<NameOfState>;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NameOfUsageAnalyzer : CSharpDiagnosticAnalyzer
    {
        private readonly NameOfRestrictionDictionary restrictionDictionary = new();
        private readonly SymbolNameOfStateDictionary symbolStateDictionary = new();

        protected override DiagnosticDescriptorStorageBase DiagnosticDescriptorStorage => NAMEDiagnosticDescriptorStorage.Instance;

        public override void Initialize(AnalysisContext context)
        {
            // Concurrent execution is disabled due to the stateful profile of the analyzer
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.ReportDiagnostics);

            RegisterNodeActions(context);
        }

        private void RegisterNodeActions(AnalysisContext context)
        {
            // Assignment of a variable could be tracked to analyze its nameof state
            // Likewise, assigment of a field/property that is specifically attributed
            var assignmentKinds = new SyntaxKind[]
            {
                SyntaxKind.SimpleAssignmentExpression,
            };

            // Invoking a method that requires an argument be passed 
            var invocationKinds = new SyntaxKind[]
            {
                SyntaxKind.InvocationExpression,
            };

            // Passing a method group as an argument, which could enforce nameof restrictions
            // which must be respected upon being called
            var methodGroupPassingKinds = new SyntaxKind[]
            {
                SyntaxKind.IdentifierName,
            };

            // Invocable member declarations contain arguments that could restrict nameof usage 
            var invocableDeclarationKinds = new SyntaxKind[]
            {
                SyntaxKind.MethodDeclaration,
                SyntaxKind.LocalFunctionStatement,
                SyntaxKind.ConstructorDeclaration,
                SyntaxKind.DelegateDeclaration,
            };

            // Fields and properties could also restrict nameof usage
            var memberVariableDeclarationKinds = new SyntaxKind[]
            {
                SyntaxKind.FieldDeclaration,
                SyntaxKind.PropertyDeclaration,
            };

            context.RegisterSyntaxNodeAction(AnalyzeAssignment, assignmentKinds);
        }

        private void AnalyzeAssignment(SyntaxNodeAnalysisContext context)
        {
            var assignmentNode = context.Node as AssignmentExpressionSyntax;
            var left = assignmentNode!.Left;
            var right = assignmentNode.Right;

            AnalyzeSimpleAssignment(context, left, right);
        }

        private void AnalyzeSimpleAssignment(SyntaxNodeAnalysisContext context, ExpressionSyntax left, ExpressionSyntax right)
        {
            var semanticModel = context.SemanticModel;

            // TODO: Evaluate case
            /*
             * // Assume definition
             * CustomString Function([ForceNameOf] string s, [ProhibitNameOf] string t) => s + t;
             * 
             * // Suspected that this throws
             * var s = Function(nameof(Function), "F");
             */

            // First ensure that the assigned expression is of type string, otherwise we don't care
            var rightHandExpressionType = semanticModel.GetTypeInfo(right);
            if (!rightHandExpressionType.MatchesExplicitlyOrImplicitly(SpecialType.System_String))
                return;

            // Now analyze the nameof state of the assigned expression
            var leftHandExpressionSymbol = semanticModel.GetSymbolInfo(left, context.CancellationToken).Symbol;
            var state = GetNameOfExpressionState(right, semanticModel, out var nameofOperations);
            symbolStateDictionary.Register(leftHandExpressionSymbol, state);
            
            // Then evaluate whether the nameof state matches that of the assigned symbol's restrictions
            AnalyzeRestrictions(context, leftHandExpressionSymbol!);
            var symbolRestrictionDictionary = restrictionDictionary[leftHandExpressionSymbol];

            foreach (var nameofOperation in nameofOperations)
            {
                var namedOperationNode = nameofOperation.Argument.Syntax;
                var alias = semanticModel.GetAliasOrSymbolInfo(namedOperationNode, context.CancellationToken);
                var symbolKind = alias.GetNamedSymbolKind();

                var restrictions = symbolRestrictionDictionary[symbolKind];

                EvaluateDiagnosticReport(restrictions, namedOperationNode, symbolKind);
            }

            EvaluateDiagnosticReport(symbolRestrictionDictionary.RestrictionForAllKinds, right, NamedSymbolKind.All);

            void EvaluateDiagnosticReport(NameOfRestrictions restrictions, SyntaxNode node, NamedSymbolKind symbolKinds)
            {
                if (!state.ValidForRestrictions(restrictions))
                {
                    var diagnosticCreator = GetDiagnosticCreator(restrictions);
                    context.ReportDiagnostic(diagnosticCreator?.Invoke(node, leftHandExpressionSymbol, symbolKinds));
                }
            }
        }

        private InvalidNameOfAssignmentDiagnosticCreator? GetDiagnosticCreator(NameOfRestrictions restrictions) => restrictions switch
        {
            NameOfRestrictions.Force             => CreateNAME0001,
            NameOfRestrictions.ForceContained    => CreateNAME0002,
            NameOfRestrictions.Prohibit          => CreateNAME0003,
            NameOfRestrictions.ProhibitContained => CreateNAME0004,
            _ => null,
        };

        private void AnalyzeRestrictions(SyntaxNodeAnalysisContext context, ISymbol symbol)
        {
            if (restrictionDictionary.ContainsKey(symbol))
                return;

            var restrictions = new NameOfRestrictionAssociation();

            var attributes = symbol.GetAttributes();
            foreach (var attribute in attributes)
            {
                var restriction = GetRestriction(attribute);
                if (restriction is null)
                    continue;

                restrictions.AddKinds(NameOfRestrictionAttributeBase.GetConstructorRestrictions(attribute), restriction.Value);
            }

            restrictionDictionary.Register(symbol, restrictions);
        }

        private static NameOfState GetNameOfExpressionState(ExpressionSyntax expression, SemanticModel model, out IEnumerable<INameOfOperation> nameofOperations)
        {
            var operation = model.GetOperation(expression);
            if (operation?.Kind is OperationKind.NameOf)
            {
                nameofOperations = new SingleElementCollection<INameOfOperation>(operation as INameOfOperation);
                return NameOfState.Whole;
            }

            var nameofNodeList = new List<INameOfOperation>();

            // Recursively attempt to find nameof expressions
            foreach (var child in expression.ChildNodes().OfType<ExpressionSyntax>())
            {
                if (GetNameOfExpressionState(child, model, out var childNameOfOperations) is not NameOfState.None)
                    nameofNodeList.AddRange(childNameOfOperations);
            }

            nameofOperations = nameofNodeList;
            return nameofNodeList.Count > 0 ? NameOfState.Contained : NameOfState.None;
        }

        private static NameOfRestrictions? GetRestriction(AttributeData attribute)
        {
            return NameOfRestrictionAttributeBase.GetRestrictionsFromAttributeName(attribute.AttributeClass!.Name);
        }
    }
}
