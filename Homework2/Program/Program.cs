using Lazy;

static int func(int x)
{
    int value = 0;
    for (int i = 0; i < 1000; ++i)
    {
        value += x;
    }
    return value;
}

LazyOneThread<int> lazyOneThread = new(() => func(1));

Console.WriteLine($"First call of Lazy - {lazyOneThread.Get()}");
Console.WriteLine($"Second call of Lazy - {lazyOneThread.Get()}");

var threads = new Thread[8];
var lazyMultiThread = new LazyMultiThread<int>(() => func(1));
for (int i = 0; i < 8; ++i)
{
    var locali = i;
    threads[i] = new Thread(() =>
    {
        Console.WriteLine($"Thread {locali}. Lazy call - {lazyMultiThread.Get()}");
    });
}

foreach (var thread in threads)
{
    thread.Start();
}
foreach (var thread in threads)
{
    thread.Join();
}
