using Microsoft.CodeAnalysis;

namespace NameOn.Core.Utilities
{
    public static class ISymbolExtensions
    {
        public static NamedSymbolKind GetNamedSymbolKind(this ISymbol symbol)
        {
            return symbol switch
            {
                INamespaceSymbol => NamedSymbolKind.Namespace,

                ITypeSymbol type => GetNamedSymbolKind(type),

                IParameterSymbol => NamedSymbolKind.Parameter,

                IEventSymbol => NamedSymbolKind.Event,
                IFieldSymbol => NamedSymbolKind.Field,
                IPropertySymbol => NamedSymbolKind.Property,
                IMethodSymbol => NamedSymbolKind.Method,

                IAliasSymbol alias => NamedSymbolKind.Alias | GetNamedSymbolKind(alias.Target),

                _ => NamedSymbolKind.None,
            };
        }

        public static NamedSymbolKind GetNamedSymbolKind(this ITypeSymbol typeSymbol)
        {
            var kind = NamedSymbolKind.None;
            if (typeSymbol.IsRecord)
                kind |= NamedSymbolKind.Record;

            kind |= typeSymbol.TypeKind switch
            {
                TypeKind.Class => NamedSymbolKind.Class,
                TypeKind.Struct => NamedSymbolKind.Struct,
                TypeKind.Interface => NamedSymbolKind.Interface,
                TypeKind.Enum => NamedSymbolKind.Enum,
                TypeKind.Delegate => NamedSymbolKind.Delegate,
                TypeKind.TypeParameter => NamedSymbolKind.GenericParameter,

                _ => NamedSymbolKind.None,
            };

            return kind;
        }
    }
}
