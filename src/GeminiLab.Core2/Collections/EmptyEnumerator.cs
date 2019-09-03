using System;
using System.Collections;
using System.Collections.Generic;

namespace GeminiLab.Core2.Collections {
    public class EmptyEnumerator<T> : IEnumerator<T> {
        public bool MoveNext() => false;

        public void Reset() { }

        public T Current => throw new InvalidOperationException();
        object IEnumerator.Current => Current!;

        public void Dispose() { }

        private EmptyEnumerator() { }

        public static EmptyEnumerator<T> Instance { get; } = new EmptyEnumerator<T>();
    }
}
