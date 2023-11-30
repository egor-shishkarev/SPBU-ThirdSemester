namespace CheckSum.Tests;

using NUnit.Framework.Constraints;
using static CheckSum;

public class CheckSumTests
{
    [TestCase("../../../../CheckSumTests/TestFiles")]
    [TestCase("../../../../CheckSumTests/TestFiles/MoreTests")]
    public void checkSumShouldReturnTheSameValueTest(string path)
    {
        var setOfValues = new HashSet<long>();
        for (int i = 0; i < 10; ++i)
        {
            setOfValues.Add(checkSum(path));
        }
        Assert.That(setOfValues.Count, Is.EqualTo(1));
    }

    [TestCase("../../../../CheckSumTests/TestFiles")]
    [TestCase("../../../../CheckSumTests/TestFiles/MoreTests")]
    public void checkSumAndCheckSumAsyncShoulgReturnTheSameValueTest(string path)
    {
        var setOfValues = new HashSet<long>();
        var setOfValuesAsync = new HashSet<long>();
        for (int i = 0; i < 10; ++i)
        {
            setOfValues.Add(checkSum(path));
            setOfValuesAsync.Add(checkSumAsync(path).Result);
        }
        var list1 = new long[1];
        var list2 = new long[1];
        setOfValues.CopyTo(list1);
        setOfValuesAsync.CopyTo(list2);
        Assert.Multiple(() =>
        {
            Assert.That(setOfValues.Count, Is.EqualTo(1));
            Assert.That(setOfValuesAsync.Count, Is.EqualTo(1));
            Assert.That(list1[0], Is.EqualTo(list2[0]));
        });
    }
}