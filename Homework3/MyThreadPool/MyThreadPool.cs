using System.Collections.Concurrent;

namespace ThreadPool;

public class MyThreadPool
{
    private BlockingCollection<Action> taskQueue;

    private MyThread[] threads;

    
    public MyThreadPool(int numberOfThreads)
    {
        if (numberOfThreads <= 0)
        {
            throw new ArgumentException("The number of threads cannot be less than or equal to 0");
        }

        threads = new MyThread[numberOfThreads];
        taskQueue = new BlockingCollection<Action>();

        for (int i = 0; i < numberOfThreads; ++i)
        {
            threads[i] = new MyThread(taskQueue);
        }
    }

    public void Submit<TResult>(Func<TResult> task) 
    {
        taskQueue.TryAdd(() => task());
    }

    public class MyThread
    {
        protected internal Thread thread;

        protected internal BlockingCollection<Action> taskQueue;

        public MyThread(BlockingCollection<Action> taskQueue)
        {
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
                    taskQueue.TryTake(out var task);
                    task();
                }
            }
        }
    }
}

