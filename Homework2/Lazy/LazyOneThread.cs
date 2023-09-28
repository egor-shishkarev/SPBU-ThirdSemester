namespace Lazy;

public class LazyOneThread<T> : ILazy<T>
{
    private T? Value;

    private readonly Func<T> Function;

    private bool IsTriggered = false;

    public LazyOneThread (Func<T> function) 
    {
        Function = function;
    }

    public T? Get()
    {
        if (!IsTriggered)
        {
            IsTriggered = true;
            Value = Function();
        }
        return Value;
    }
}
