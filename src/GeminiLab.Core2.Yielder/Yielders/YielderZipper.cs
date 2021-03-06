using System;

namespace GeminiLab.Core2.Yielder.Yielders {
    internal class YielderZipper<TA, TB, TR> : IYielder<TR> {
        private readonly IYielder<TA> _yielderA;
        private readonly IYielder<TB> _yielderB;
        private readonly Func<TA, TB, TR> _func;

        public YielderZipper(IYielder<TA> yielderA, IYielder<TB> yielderB, Func<TA, TB, TR> func) {
            _yielderA = yielderA;
            _yielderB = yielderB;
            _func = func;
        }

        public TR Next() => _func(_yielderA.Next(), _yielderB.Next());
    }
}
