using Microsoft.CodeAnalysis.CSharp;

namespace NameOn.Core
{
    public enum AttributeListTarget
    {
        Default,

        Assembly,
        Module,
        Field,
        Event,
        Method,
        Param,
        Property,
        Return,
        Type,
    }

    internal static class AttributeListTargetCaches
    {
        public static readonly InterlinkedDictionary<AttributeListTarget, SyntaxKind> SyntaxKindInterlinks = new();

        static AttributeListTargetCaches()
        {
            foreach (var targetField in typeof(AttributeListTarget).GetFields())
            {
                var targetValue = (AttributeListTarget)targetField.GetRawConstantValue();
                if (targetValue is AttributeListTarget.Default)
                    continue;

                var syntaxKindValue = typeof(SyntaxKind).GetField($"{targetField.Name}Keyword").GetRawConstantValue();
                SyntaxKindInterlinks.Add(targetValue, (SyntaxKind)syntaxKindValue);
            }
        }
    }

    public static class AttributeListTargetExtensions
    {
        public static SyntaxKind GetSyntaxKind(this AttributeListTarget attributeListTarget)
        {
            return AttributeListTargetCaches.SyntaxKindInterlinks.ValueOrDefault(attributeListTarget);
        }
        public static AttributeListTarget GetAttributeListTarget(this SyntaxKind syntaxKind)
        {
            return AttributeListTargetCaches.SyntaxKindInterlinks.ValueOrDefault(syntaxKind);
        }
    }
}
