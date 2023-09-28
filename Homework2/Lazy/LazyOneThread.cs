namespace Lazy;

public class LazyOneThread<T> : ILazy<T>
{
    private T? value;

    private readonly Func<T> supplier;

    private bool isTriggered = false;

    public LazyOneThread (Func<T> supplier) 
    {
        this.supplier = supplier;
    }

    public T? Get()
    {
        if (!isTriggered)
        {
            isTriggered = true;
            value = supplier();
        }
        return value;
    }
}
