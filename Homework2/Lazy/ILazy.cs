namespace Lazy;

/// <summary>
/// Interface for custom implementation of lazy initialization.
/// </summary>
/// <typeparam name="T">Return type.</typeparam>
public interface ILazy <T>
{
    /// <summary>
    /// Method, which at first call calculate the Func<T>, at next calls just return value, which was calculated at first call.
    /// </summary>
    /// <returns>Result of Func<T> call.</returns>
    T? Get();
}
