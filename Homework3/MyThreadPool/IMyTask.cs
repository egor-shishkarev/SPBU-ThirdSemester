namespace ThreadPool;

public interface IMyTask<TResult>
{
    // Returns true if taks is completed
    public bool IsCompleted { get; }

    // Returns result of task
    public TResult Result { get; }

    // Allows you to apply a task to a function of type Func<Result, TNewResult>
    public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> function);
}