using Microsoft.CodeAnalysis;
using NameOn.Core;
using RoseLynn.Utilities;
using System.Collections.Generic;

namespace NameOn
{
    public class SymbolNameOfStateDictionary
    {
        private Dictionary<ISymbol, NameOfState> dictionary = new(SymbolEqualityComparer.Default);

        public void Register(ISymbol symbol, NameOfState state) => dictionary.AddOrSet(symbol, state);

        public NameOfState this[ISymbol symbol] => dictionary[symbol];
    }
}
