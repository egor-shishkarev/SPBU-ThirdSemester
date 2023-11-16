﻿using ThreadPool;

var threadPool = new MyThreadPool(4);

var task1 = threadPool.Submit(() => { Thread.Sleep(1000); return 1; });
var task2 = threadPool.Submit(() => { Thread.Sleep(1000); return 2; });
var task3 = threadPool.Submit(() => { Thread.Sleep(1000); return 3; });
var task4 = threadPool.Submit(() => { Thread.Sleep(1000); return 4; });
threadPool.Shutdown();
Console.WriteLine(task1.Result);
Console.WriteLine(task2.Result);
Console.WriteLine(task3.Result);
Console.WriteLine(task4.Result);
