namespace Lazy;

public class LazyMultiThread<T> : ILazy<T>
{
    private T? value;

    private Func<T> supplier;

    private volatile bool isTriggered = false;

    private object lockObject = new();

    public LazyMultiThread (Func<T> supplier)
    {
        this.supplier = supplier;
    } 

    public T? Get()
    {
        if (!isTriggered)
        {
            lock (lockObject)
            {
                if (!isTriggered)
                {
                    value = supplier();
                    isTriggered = true;
                }
            }
        }
        return value;
    }
}
