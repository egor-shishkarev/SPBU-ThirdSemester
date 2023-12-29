namespace Lazy.Test;

using System.Text;

public class LazyOneThreadTest
{
    private static readonly Func<object> intFunction = () => 
    {
        int x = 0; 
        for (int i = 0; i < 10000; ++i) 
        {
            x += i;
        }
        return x;
    };

    private static readonly Func<object> stringFunction = () =>
    {
        StringBuilder x = new(); 
        for (int i = 0; i < 10000; ++i)
        {
            x.Append('x');
        }
        return x.ToString();
    };

    private static readonly Func<List<object>> objectFunction = () =>
    {
        List<object> x = new(); 
        for (int i = 0; i < 10000; ++i)
        {
            x.Add(new object());
        }
        return x;
    };

    private static readonly Func<string>? nullFunction = () => null!;

    [TestCaseSource(nameof(GetArrayOfLazy), new object[] { 0 })]
    public void LazyShouldReturnSameResultTest(ILazy<object> lazy)
    {
        var firstResult = lazy.Get();
        var secondResult = lazy.Get();
        var thirdResult = lazy.Get();

        Assert.Multiple(() =>
        {
            Assert.That(firstResult, Is.EqualTo(secondResult));
            Assert.That(secondResult, Is.EqualTo(thirdResult));
        });
    }

    [TestCaseSource(nameof(GetArrayOfLazy), new object[] { 1 })]
    public void LazyWithStringFunctionShouldReturnSameResultTest(ILazy<object> lazy)
    {
        var firstResult = lazy.Get();
        var secondResult = lazy.Get();
        var thirdResult = lazy.Get();

        Assert.Multiple(() =>
        {
            Assert.That(firstResult, Is.EqualTo(secondResult));
            Assert.That(secondResult, Is.EqualTo(thirdResult));
        });
    }

    [TestCaseSource(nameof(GetArrayOfLazy), new object[] { 2 })]
    public void LazyWithObjectFunctionShouldReturnSameResultTest(ILazy<object> lazy)
    {
        var firstResult = lazy.Get();
        var secondResult = lazy.Get();
        var thirdResult = lazy.Get();

        Assert.Multiple(() =>
        {
            Assert.That(firstResult, Is.EqualTo(secondResult));
            Assert.That(secondResult, Is.EqualTo(thirdResult));
        });
    }

    [TestCaseSource(nameof(GetArrayOfLazy), new object[] { 3 })]
    public void LazyWithNullFunctionShouldReturnSameResultTest(ILazy<object> lazy)
    {
        var firstResult = lazy.Get();
        var secondResult = lazy.Get();
        var thirdResult = lazy.Get();

        Assert.Multiple(() =>
        {
            Assert.That(firstResult, Is.EqualTo(secondResult));
            Assert.That(secondResult, Is.EqualTo(thirdResult));
        });
    }

    private static TestCaseData[] GetArrayOfLazy(int numberOfFunction)
    {
        var arrayOfLazy = new TestCaseData[2];
        Func<object>[] arrayOfFunctions = new[] { intFunction, stringFunction, objectFunction, nullFunction! };
        arrayOfLazy[0] = new TestCaseData(new LazyOneThread<object>(arrayOfFunctions[numberOfFunction]));
        arrayOfLazy[1] = new TestCaseData(new LazyMultiThread<object>(arrayOfFunctions[numberOfFunction]));

        return arrayOfLazy;
    }

}