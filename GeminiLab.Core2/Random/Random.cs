namespace GeminiLab.Core2.Random {
    // contract:
    // Next() return all possible values of TResult
    public interface IRNG<out TResult> {
        TResult Next();
    }

    public interface IPRNG<out TResult, in TSeed> : IRNG<TResult> {
        void Seed(TSeed seed);
    }

    public interface IPRNG<T> : IPRNG<T, T> { }
}