namespace ThreadPool.Tests;

public class MyThreadPoolTests
{
    [Test]
    public void CannotCreateThreadPoolWithLessThanOrEqualZeroThreadsTest()
    {
        Assert.Multiple(() =>
        {
            Assert.Throws<ArgumentException>(() => new MyThreadPool(0));
            Assert.Throws<ArgumentException>(() => new MyThreadPool(-1));
        });
    }

    [TestCase(3)]
    [TestCase(6)]
    [TestCase(12)]
    // Тщательнее проверить количество потоков, использовать n задач с resetEvent-ами, и проверять, что все потоки действительно заняты
    // Или заполнить пул n задачами и добавить одну + проверить что очередь не пуста
    public void ThreadPoolMustContainAsMuchThreadsAsWePassInTheConstructorTest(int numberOfThreads)
    {
        var threadPool = new MyThreadPool(numberOfThreads);

        Assert.That(threadPool.CountOfThreads, Is.EqualTo(numberOfThreads));
    }

    [Test]
    public void ThreadPoolShouldReturnMyTaskWithCompletedStatusAndCalculatedResult()
    {
        var threadPool = new MyThreadPool(3);
        var firstTask = threadPool.Submit(() => { var acc = 0; for (int i = 1; i < 10; ++i) { acc += i; }; return acc; });
        var secondTask = threadPool.Submit(() => { var accStr = ""; accStr += "Hello"; accStr += " World"; accStr += "!"; return accStr; });
        var thirdTask = threadPool.Submit(() => { return (1 < 2); });
        threadPool.Shutdown();

        Assert.Multiple(() =>
        {
            Assert.That(firstTask.IsCompleted && firstTask.Result == 45, Is.True);
            Assert.That(secondTask.IsCompleted && secondTask.Result == "Hello World!", Is.True);
            Assert.That(thirdTask.IsCompleted && thirdTask.Result, Is.True);
        });
    }

    [Test]
    public void AttemptToAddNewTaskAfterShutdownThreadPoolShouldThrowExceptionTest()
    {
        var threadPool = new MyThreadPool(2);
        threadPool.Submit(() => 1);
        threadPool.Shutdown();
        Assert.Throws<OperationCanceledException>(() => threadPool.Submit(() => 1));
    }

    [Test]
    public void ContinueWithShoulWorkAsExpectedTest()
    {
        var threadPool = new MyThreadPool(3);
        var task = threadPool.Submit(() => 2 * 2).ContinueWith(x => x * 2);
        threadPool.Shutdown();

        Assert.That(task.IsCompleted && task.Result == 8, Is.True);
    }

    // Добавить тесты на конкурентный доступ к пулу
}