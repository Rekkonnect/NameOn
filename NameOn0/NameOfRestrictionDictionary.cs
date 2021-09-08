using Microsoft.CodeAnalysis;
using NameOn.Core;
using RoseLynn.Utilities;
using System.Collections.Generic;

namespace NameOn
{
    public class NameOfRestrictionDictionary
    {
        private Dictionary<ISymbol, NameOfRestrictionType> dictionary = new(SymbolEqualityComparer.Default);

        public void Register(ISymbol symbol, NameOfRestrictionType restrictionType) => dictionary.TryAddPreserve(symbol, restrictionType);

        public NameOfRestrictionType this[ISymbol symbol] => dictionary[symbol];
    }
}
