using System.Text;

namespace Lazy.Test;

public class LazyOneThreadTest
{
    private static readonly Func<int> intFunction = () => { int x = 0; for (int i = 0; i < 10000; ++i) { x += i; }; return x; };

    private static readonly Func<string> stringFunction = () => { StringBuilder x = new(); for (int i = 0; i < 10000; ++i) { x.Append('x'); }; return x.ToString(); };

    private static readonly Func<List<object>> objectFunction = () => { List<object> x = new(); for (int i = 0; i < 10000; ++i) { x.Add(new object()); } return x; };

    private static readonly Func<string>? nullFunction = () => null!;

    [Test]
    public void LazyWithIntFunctionShouldReturnSameResultTest()
    {
        var lazy = new LazyOneThread<int>(intFunction);
        var firstResult = lazy.Get();
        var secondResult = lazy.Get();
        var thirdResult = lazy.Get();

        Assert.Multiple(() =>
        {
            Assert.That(firstResult, Is.EqualTo(secondResult));
            Assert.That(secondResult, Is.EqualTo(thirdResult));
        });
    }

    [Test]
    public void LazyWithStringFunctionShouldReturnSameResultTest()
    {
        var lazy = new LazyOneThread<string>(stringFunction);
        var firstResult = lazy.Get();
        var secondResult = lazy.Get();
        var thirdResult = lazy.Get();

        Assert.Multiple(() =>
        {
            Assert.That(firstResult, Is.EqualTo(secondResult));
            Assert.That(secondResult, Is.EqualTo(thirdResult));
        });
    }

    [Test]
    public void LazyWithObjectFunctionShouldReturnSameResultTest()
    {
        var lazy = new LazyOneThread<object>(objectFunction);
        var firstResult = lazy.Get();
        var secondResult = lazy.Get();
        var thirdResult = lazy.Get();

        Assert.Multiple(() =>
        {
            Assert.That(firstResult, Is.EqualTo(secondResult));
            Assert.That(secondResult, Is.EqualTo(thirdResult));
        });
    }

    [Test]
    public void LazyWithNullFunctionShouldReturnSameResultTest()
    {
        var lazy = new LazyOneThread<string>(nullFunction!);
        var firstResult = lazy.Get();
        var secondResult = lazy.Get();
        var thirdResult = lazy.Get();

        Assert.Multiple(() =>
        {
            Assert.That(firstResult, Is.EqualTo(secondResult));
            Assert.That(secondResult, Is.EqualTo(thirdResult));
        });
    }

}