using System.Collections.Concurrent;

namespace ThreadPool;

public class MyThreadPool // Нарушен порядок элементов в классах (Проверить) [Поля, конструктор, свойства, методы, классы ]
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

    public int CountOfThreads => threads.Length;

    public IMyTask<TResult> Submit<TResult>(Func<TResult> task)
    {
        if (task == null)
        {
            throw new ArgumentNullException(nameof(task), "Task can't be null");
        }
        cancellationTokenSource.Token.ThrowIfCancellationRequested();

        var newTask = new MyTask<TResult>(task, cancellationTokenSource.Token, taskQueue);
        taskQueue.Add(newTask.Start); // Залочить + добавить lock на очередь задач
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

    private class MyThread // Сделать класс приватным
    {
        private readonly Thread thread;

        private readonly BlockingCollection<Action> taskQueue;

        private readonly CancellationToken token;

        private bool IsWorking;

        public MyThread(BlockingCollection<Action> taskQueue, CancellationToken token)
        {
            this.token = token;
            this.taskQueue = taskQueue;
            thread = new Thread(Start);
            thread.Start();
        }

        public void Start()
        {
            while (!token.IsCancellationRequested) // Реализовать ту же идею с waitOne, что и в result - потоки стоят на ResetEvent в
                // Отсутствии задач, чтобы не грузить проц
            {
                if (taskQueue.Count > 0)
                {
                    if (taskQueue.TryTake(out var task)) 
                    {
                        this.IsWorking = true;
                        task();
                        this.IsWorking = false;
                    }
                } 
                else if (token.IsCancellationRequested)
                {
                    break;
                }
            }
        }

        public void Join()
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

        private readonly CancellationToken token;

        private readonly BlockingCollection<Action> nextTasks;

        private readonly BlockingCollection<Action> taskQueue;

        public TResult Result
        {
            get
            {
                resetEvent.WaitOne();
                if (exception != null)
                {
                    throw new AggregateException(exception);
                }
                return result;
            }
        }

        public MyTask(Func<TResult> task, CancellationToken token, BlockingCollection<Action> taskQueue) // Лучше передавать экземпляр пула и 
            // добавлять с помощью submit + ругается компилятор
        {
            IsCompleted = false;
            Func = task;
            this.token = token;
            nextTasks = new();
            this.taskQueue = taskQueue;
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
            foreach (var task in nextTasks)
            {
                taskQueue.Add(task);
            }
        }

        public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> task)
        {
            token.ThrowIfCancellationRequested();
            var newTask = new MyTask<TNewResult>(() => task(Result), token, taskQueue);
            if (IsCompleted)
            {
                taskQueue.Add(newTask.Start);
                return newTask;
            }

            nextTasks.Add(newTask.Start);
            return newTask;
        }
    }
}

