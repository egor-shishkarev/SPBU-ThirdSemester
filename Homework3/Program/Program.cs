using ThreadPool;

var threadPool = new MyThreadPool(4);

var task1 = threadPool.Submit(() => { Thread.Sleep(1000); return 1; });
var task2 = threadPool.Submit(() => { Thread.Sleep(1000); return 2; });
var task3 = threadPool.Submit(() => { Thread.Sleep(1000); return 3; });
var task4 = threadPool.Submit(() => { Thread.Sleep(1000); return 4; });
Console.WriteLine(task1.Result);
Console.WriteLine(task2.Result);
Console.WriteLine(task3.Result);
Console.WriteLine(task4.Result);

var task12 = threadPool.Submit(() => { Thread.Sleep(1000); return 1; }).Result;
var task22 = threadPool.Submit(() => { Thread.Sleep(1000); return 2; }).Result;
var task32 = threadPool.Submit(() => { Thread.Sleep(1000); return 3; }).Result;
var task42 = threadPool.Submit(() => { Thread.Sleep(1000); return 4; }).Result;
Console.WriteLine(task12);
Console.WriteLine(task22);
Console.WriteLine(task32);
Console.WriteLine(task42);

