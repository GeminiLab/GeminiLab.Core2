using System;

namespace GeminiLab.Core2.Yielder.FiniteYielders {
    internal class FiniteYielderTaker<T> : IFiniteYielder<T> {
        private readonly IFiniteYielder<T> _source;
        private readonly int _limit;
        private int _count;

        public FiniteYielderTaker(IFiniteYielder<T> source, int limit) {
            _source = source;
            _limit = limit;

            _count = 0;
        }

        public bool HasNext() {
            return _source.HasNext() && _count < _limit;
        }

        public T Next() {
            if (!_source.HasNext()) throw new InvalidOperationException();

            ++_count;
            return _source.Next();
        }
    }
}