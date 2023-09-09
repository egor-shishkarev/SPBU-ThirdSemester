using Matrices;
using System.Diagnostics;

const string outputPath = "../../../../ResultMatrix.txt";

if (args.Length < 2)
{
    Console.Error.WriteLine("Пути до файлов не были введены!");
    return -1;
}

var firstMatrix = new Matrix(args[0]);
var secondMatrix = new Matrix(args[1]);

Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();
var resultMatrixWithMultithreading = Matrix.MultiplicateWithMultithreading(firstMatrix, secondMatrix);
stopwatch.Stop();
Console.WriteLine(stopwatch.ElapsedMilliseconds);

Stopwatch stopwatch1 = new Stopwatch();
stopwatch1.Start();
var resultMatrix = Matrix.Multiplicate(firstMatrix, secondMatrix);
stopwatch1.Stop();
Console.WriteLine(stopwatch1.ElapsedMilliseconds);

Console.WriteLine($"Результат прироста эффективности - {(double)stopwatch1.ElapsedMilliseconds / stopwatch.ElapsedMilliseconds}");
Matrix.Write(outputPath, resultMatrixWithMultithreading);

Console.WriteLine($"Результат вычисления находится в файле {outputPath[12..]}");

return 0;
