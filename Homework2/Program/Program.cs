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

LazyOneThread<int> lazy = new(() => func(1));

lazy.Get();
lazy.Get();

var threads = new Thread[8];
var lazy2 = new LazyMultiThread<int>(() => func(1));
for (int i = 0; i < 8; ++i)
{
    var locali = i;
    threads[i] = new Thread(() =>
    {
        Console.WriteLine(lazy2.Get());
        Console.WriteLine($"Thread {locali} is working!");
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