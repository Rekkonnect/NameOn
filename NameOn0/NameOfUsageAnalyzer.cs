using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using RoseLynn.Analyzers;

// The analyzer should not be run concurrently due to the state that it preserves
#pragma warning disable RS1026 // Enable concurrent execution

namespace NameOn
{
    public class NameOfUsageAnalyzer : CSharpDiagnosticAnalyzer
    {
        private readonly NameOfRestrictionDictionary restrictionDictionary = new();
        private readonly NameOfRestrictionDictionary restrictionDictionary = new();

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

        private void AnalyzeGenericNameOrFunctionCall(SyntaxNodeAnalysisContext context)
        {
            var semanticModel = context.SemanticModel;

            var node = context.Node;
            var symbolInfo = semanticModel.GetSymbolInfo(node);
            var symbol = symbolInfo.Symbol;
        }
        private void AnalyzeAssignment(SyntaxNodeAnalysisContext context)
        {
            var semanticModel = context.SemanticModel;

            var assignmentNode = context.Node as AssignmentExpressionSyntax;
            var left = assignmentNode.Left;
            var right = assignmentNode.Right;

            // First ensure that the assigned expression is of type string, otherwise we don't care
            var rightHandExpressionType = semanticModel.GetTypeInfo(right);
            if (rightHandExpressionType.Type?.SpecialType is not SpecialType.System_String)
                return;

            // Now analyze whether the expression of the assigned symbol has explicit declarations
            var leftHandExpressionSymbol = semanticModel.GetSymbolInfo(left, context.CancellationToken).Symbol;
            switch (left)
            {
                case MemberAccessExpressionSyntax memberAccess:
                    var memberSymbol = semanticModel.GetSymbolInfo(memberAccess, context.CancellationToken).Symbol;

                    break;

                case IdentifierNameSyntax identifierName:
                    var identifierSymbol = semanticModel.GetSymbolInfo(identifierName).Symbol;
                    
                    break;
            }
        }

        private void AnalyzeProfileRelatedDeclaration(SyntaxNodeAnalysisContext context)
        {
            var symbol = context.SemanticModel.GetDeclaredSymbol(context.Node) as INamedTypeSymbol;
        }
    }
}
