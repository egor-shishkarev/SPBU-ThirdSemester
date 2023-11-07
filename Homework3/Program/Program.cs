using ThreadPool;

var threadPool = new MyThreadPool(3);

threadPool.Submit(() => { Thread.Sleep(1000); Console.WriteLine("First"); return 0; });
threadPool.Submit(() => { Thread.Sleep(2000); Console.WriteLine("Second"); return 0; });
threadPool.Submit(() => { Thread.Sleep(4000); Console.WriteLine("Third"); return 0; });

