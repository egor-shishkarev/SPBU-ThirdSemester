namespace Lazy;

/// <summary>
/// Lazy implementation with single thread.
/// </summary>
/// <typeparam name="T">Return type.</typeparam>
public class LazyOneThread<T> : ILazy<T>
{
    /// <summary>
    /// After first call of Lazy contains result of Func<T>.
    /// </summary>
    private T? value;

    /// <summary>
    /// Function, which we want to lazy initializate. 
    /// </summary>
    private readonly Func<T> supplier;

    /// <summary>
    /// Trigger to notify if supplier was calculated.
    /// </summary>
    private bool isTriggered = false;

    /// <summary>
    /// Constructor for Lazy object.
    /// </summary>
    /// <param name="supplier">Function, which we want to lazy initializate.</param>
    public LazyOneThread (Func<T> supplier) 
    {
        this.supplier = supplier;
    }

    /// <summary>
    /// Method, which at first call calculate the Func<T>, at next calls just return value, which was calculated at first call.
    /// </summary>
    /// <returns>Result of Func<T> call.</returns>
    public T? Get()
    {
        if (!isTriggered)
        {
           value = supplier();
           isTriggered = true;
        }
        return value;
    }
}
