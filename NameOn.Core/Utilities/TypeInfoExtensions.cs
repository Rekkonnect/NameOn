using Microsoft.CodeAnalysis;

namespace NameOn.Core.Utilities
{
    public static class TypeInfoExtensions
    {
        /// <summary>Determines whether a <see cref="TypeInfo"/> result matches a given <see cref="SpecialType"/>.</summary>
        /// <param name="typeInfo">The <seealso cref="TypeInfo"/> whose <seealso cref="SpecialType"/> to attempt to match.</param>
        /// <param name="specialType">The desired matching <see cref="SpecialType"/>.</param>
        /// <returns><see langword="true"/> if <seealso cref="TypeInfo.Type"/> or <seealso cref="TypeInfo.ConvertedType"/> match <paramref name="specialType"/>, otherwise <see langword="false"/>.</returns>
        public static bool MatchesExplicitlyOrImplicitly(this TypeInfo typeInfo, SpecialType specialType)
        {
            return typeInfo.Type?.SpecialType == specialType
                || typeInfo.ConvertedType?.SpecialType == specialType;
        }
    }
}
