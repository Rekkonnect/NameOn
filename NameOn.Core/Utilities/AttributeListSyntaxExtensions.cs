using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NameOn.Core.Utilities
{
    public static class AttributeListSyntaxExtensions
    {
        public static AttributeListTarget GetTarget(this AttributeListSyntax attributeList)
        {
            var targetNode = attributeList.Target;
            if (targetNode is null)
                return AttributeListTarget.Default;

            return targetNode.Identifier.Kind().GetAttributeListTarget();
        }
        public static bool HasTarget(this AttributeListSyntax attributeList, AttributeListTarget target)
        {
            var targetNode = attributeList.Target;
            if (targetNode is null)
                return target is AttributeListTarget.Default;

            return targetNode.Identifier.IsKind(target.GetSyntaxKind());
        }
    }
}
