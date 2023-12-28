namespace Lazy.Test;

public class LazyMultiThreadTest
{
    private static readonly ManualResetEvent startEvent = new (false);
    [Test]
    public void LazyShouldReturnSameResultsFromDifferentThreadsTest()
    {
        int sum = 0;
        var lazy = new LazyMultiThread<int>(() => Interlocked.Increment(ref sum));
        var threads = new Thread[10];
        var resultInThreads = new int[threads.Length];
        for (int i = 0; i < threads.Length; ++i)
        {
            var locali = i;
            threads[i] = new Thread(() => {
                startEvent.WaitOne();
                resultInThreads[locali] = lazy.Get();
            });
        }

        foreach (var thread in threads)
        {
            thread.Start();
        }

        startEvent.Set();

        foreach (var thread in threads)
        {
            thread.Join();
        }

        Assert.That(sum, Is.EqualTo(1));
        for (int i = 0; i < 9; ++i)
        {
            Assert.That(resultInThreads[i], Is.EqualTo(resultInThreads[i + 1]));
        }
    }
}