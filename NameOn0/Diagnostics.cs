using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static NameOn.NAMEDiagnosticDescriptorStorage;

namespace NameOn
{
    internal static class Diagnostics
    {
        public static Diagnostic CreateNAME0001(SyntaxNode node, ISymbol originalDefinition, ITypeSymbol argumentType)
        {
            return Diagnostic.Create(Instance.NAME0001_Rule, node?.GetLocation(), originalDefinition.ToDisplayString(), argumentType.ToDisplayString());
        }
        public static Diagnostic CreateNAME0002(AttributeArgumentSyntax attributeArgumentSyntaxNode, ITypeParameterSymbol typeParameter, ITypeSymbol argumentType)
        {
            return Diagnostic.Create(Instance.NAME0002_Rule, attributeArgumentSyntaxNode?.GetLocation(), typeParameter.ToDisplayString(), argumentType.ToDisplayString());
        }
    }
}
