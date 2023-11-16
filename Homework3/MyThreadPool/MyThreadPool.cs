using System.Collections.Concurrent;

namespace ThreadPool;

public class MyThreadPool
{
    private readonly BlockingCollection<Action> taskQueue;

    private readonly MyThread[] threads;

    private readonly CancellationTokenSource cancellationTokenSource;

    public MyThreadPool(int numberOfThreads)
    {
        if (numberOfThreads <= 0)
        {
            throw new ArgumentException("The number of threads cannot be less than or equal to 0");
        }

        threads = new MyThread[numberOfThreads];
        taskQueue = new BlockingCollection<Action>();
        cancellationTokenSource = new();

        for (int i = 0; i < numberOfThreads; ++i)
        {
            threads[i] = new MyThread(taskQueue, cancellationTokenSource.Token);
        }
    }

    public IMyTask<TResult> Submit<TResult>(Func<TResult> task) 
    {
        cancellationTokenSource.Token.ThrowIfCancellationRequested();

        var newTask = new MyTask<TResult>(task);
        taskQueue.Add(newTask.Start);
        return newTask;
    }

    public void Shutdown()
    {
        cancellationTokenSource.Cancel();
        foreach (var thread in threads)
        {
            thread.Join();
        }
    }

    public class MyThread
    {
        private readonly Thread thread;

        private readonly BlockingCollection<Action> taskQueue;

        private readonly CancellationToken token;

        public MyThread(BlockingCollection<Action> taskQueue, CancellationToken token)
        {
            this.token = token;
            this.taskQueue = taskQueue;
            thread = new Thread(Start);
            thread.Start();
        }

        private void Start()
        {
            while (true)
            {
                if (taskQueue.Count > 0)
                {
                    if (taskQueue.TryTake(out var task)) 
                    {
                        task();
                    }
                } 
                else if (token.IsCancellationRequested)
                {
                    break;
                }
            }
        }

        protected internal void Join()
        {
            thread.Join();
        }
    }

    private class MyTask<TResult> : IMyTask<TResult>
    {
        public bool IsCompleted { get; private set; }

        public Func<TResult> Func { get; private set; }

        private Exception? exception = null;

        private TResult result;

        private readonly ManualResetEvent resetEvent = new(false);

        public TResult Result
        {
            get
            {
                if (!IsCompleted)
                {
                    resetEvent.WaitOne();
                }
                
                if (exception != null)
                {
                    throw new AggregateException(exception);
                }
                return result;
            }
        }

        public MyTask(Func<TResult> task)
        {
            IsCompleted = false;
            Func = task;
        }

        public void Start()
        {
            try
            {
                result = Func();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            resetEvent.Set();
            IsCompleted = true;
        }
    }
}

