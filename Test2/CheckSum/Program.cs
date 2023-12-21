using System.Diagnostics;
using static CheckSum.CheckSum;

if (args.Length < 1)
{
    Console.Error.WriteLine("No arguments were passed!");
    return -1;
}

if (args.Length > 1)
{
    Console.Error.WriteLine("Too many arguments!");
    return -1;
}

var path = args[0];
if (string.IsNullOrEmpty(path))
{
    Console.Error.WriteLine("Path to the file/directory mustn't be null or empty!");
    return -1;
}

var stopwatch = new Stopwatch();
stopwatch.Restart();
try
{
    Console.WriteLine(checkSumAsync(path).Result);
} 
catch (Exception ex)
{
    Console.Error.WriteLine(ex.Message);
    return -1;
}
stopwatch.Stop();
Console.WriteLine($"Секунды на асинхронное нахождение check-суммы: {stopwatch.ElapsedMilliseconds / 1000}");

stopwatch.Restart();
try
{
    Console.WriteLine(checkSum(path));
}
catch (Exception ex)
{
    Console.Error.WriteLine(ex.Message);
    return -1;
}
stopwatch.Stop();
Console.WriteLine($"Секунды на синхронное нахождение check-суммы: {stopwatch.ElapsedMilliseconds / 1000}");
return 0;
