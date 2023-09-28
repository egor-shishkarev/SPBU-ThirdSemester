namespace Lazy.Test;

public class LazyMultiThreadTest
{
    [Test]
    public void LazyShouldReturnSameResultsFromDifferentThreadsTest()
    {
        int sum = 0;
        Func<int> function = () => { for (int i = 0; i < 10000; ++i) { sum += 1; }; return sum; };
        var lazy = new LazyMultiThread<int>(function);
        var threads = new Thread[10];
        var resultInThreads = new int[threads.Length];
        for (int i = 0; i < threads.Length; ++i)
        {
            var locali = i;
            threads[locali] = new Thread(() => resultInThreads[locali] = lazy.Get());
        }

        foreach (var thread in threads)
        {
            thread.Start();
        }
        foreach (var thread in threads)
        {
            thread.Join();
        }

        Assert.That(sum, Is.EqualTo(10000));
        for (int i = 0; i < 9; ++i)
        {
            Assert.That(resultInThreads[i], Is.EqualTo(resultInThreads[i + 1]));
        }
    }
}