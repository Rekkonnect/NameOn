#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace NameOn.Core
{
    public class InterlinkedDictionary<T1, T2>
    {
        private readonly Dictionary<T1, T2> t1Dictionary = new();
        private readonly Dictionary<T2, T1> t2Dictionary = new();

        public int Count => t1Dictionary.Count;

        public IEnumerable<T1> Values1 => t1Dictionary.Keys;
        public IEnumerable<T2> Values2 => t2Dictionary.Keys;
        public IEnumerable<(T1, T2)> ValuePairs => t1Dictionary.Keys.Zip(t2Dictionary.Keys, Selectors.MakeTuple);

        public bool Contains(T1 t1Value) => t1Dictionary.ContainsKey(t1Value);
        public bool Contains(T2 t2Value) => t2Dictionary.ContainsKey(t2Value);

        public void Add(T1 t1Value, T2 t2Value)
        {
            t1Dictionary.Add(t1Value, t2Value);
            t2Dictionary.Add(t2Value, t1Value);
        }

        public bool Remove(T1 t1Value)
        {
            if (!t1Dictionary.TryGetValue(t1Value, out var t2Value))
                return false;

            return Remove(t1Value, t2Value);
        }
        public bool Remove(T2 t2Value)
        {
            if (!t2Dictionary.TryGetValue(t2Value, out var t1Value))
                return false;

            return Remove(t1Value, t2Value);
        }

        private bool Remove(T1 t1Value, T2 t2Value)
        {
            t1Dictionary.Remove(t1Value);
            t2Dictionary.Remove(t2Value);
            return true;
        }

        public void Clear()
        {
            t1Dictionary.Clear();
            t2Dictionary.Clear();
        }

        public bool TryGetValue(T1 t1Key, out T2 t2Value)
        {
            return t1Dictionary.TryGetValue(t1Key, out t2Value);
        }
        public bool TryGetValue(T2 t2Key, out T1 t1Value)
        {
            return t2Dictionary.TryGetValue(t2Key, out t1Value);
        }

        public T2? ValueOrDefault(T1 key)
        {
            TryGetValue(key, out var value);
            return value;
        }
        public T1? ValueOrDefault(T2 key)
        {
            TryGetValue(key, out var value);
            return value;
        }

        public T2 this[T1 t1Key]
        {
            get => t1Dictionary[t1Key];
            set => ChangeValue(t1Dictionary, t2Dictionary, t1Key, value);
        }
        public T1 this[T2 t2Key]
        {
            get => t2Dictionary[t2Key];
            set => ChangeValue(t2Dictionary, t1Dictionary, t2Key, value);
        }

        private static void ChangeValue<TFocused, TOther>(Dictionary<TFocused, TOther> focusedDictionary, Dictionary<TOther, TFocused> otherDictionary, TFocused key, TOther value)
        {
            focusedDictionary[key] = value;
            otherDictionary.Remove(value);
            otherDictionary.Add(value, key);
        }
    }
}
