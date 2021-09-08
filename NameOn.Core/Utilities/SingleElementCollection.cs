using System;
using System.Collections;
using System.Collections.Generic;

namespace NameOn.Core.Utilities
{
    public struct SingleElementCollection<T> : IReadOnlyCollection<T>
    {
        private readonly T element;

        public int Count => 1;

        public SingleElementCollection(T singleElement)
        {
            element = singleElement;
        }

        public IEnumerator<T> GetEnumerator() => new SingleElementEnumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private struct SingleElementEnumerator : IEnumerator<T>
        {
            private readonly SingleElementCollection<T> instance;
            private EnumerationState state;

            public SingleElementEnumerator(SingleElementCollection<T> collection)
            {
                instance = collection;
                state = EnumerationState.Before;
            }

            public T Current
            {
                get
                {
                    if (state is EnumerationState.After)
                        throw new InvalidOperationException("The single element has already been enumerated.");

                    if (state is EnumerationState.Before)
                        throw new InvalidOperationException("The enumeration has not yet begun.");

                    return instance.element;
                }
            }
            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (state is EnumerationState.After)
                    return false;

                state++;
                return state < EnumerationState.After;
            }
            public void Reset()
            {
                state = EnumerationState.Before;
            }

            public void Dispose() { }

            private enum EnumerationState : byte
            {
                Before,
                Current,
                After
            }
        }
    }
}
