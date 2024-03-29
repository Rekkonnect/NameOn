﻿using Microsoft.CodeAnalysis;
using RoseLynn.Utilities;
using System.Collections.Generic;

namespace NameOn
{
    public class SymbolDictionary<TValue>
    {
        private readonly Dictionary<ISymbol, TValue> dictionary = new(SymbolEqualityComparer.Default);

        public void Register(ISymbol symbol, TValue restrictionType) => dictionary.TryAddPreserve(symbol, restrictionType);

        public bool ContainsKey(ISymbol symbol) => dictionary.ContainsKey(symbol);

        public TValue this[ISymbol symbol] => dictionary.ValueOrDefault(symbol);
    }
}
