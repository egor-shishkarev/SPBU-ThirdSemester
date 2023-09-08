using Matrices;

const string outputPath = "../../../../ResultMatrix.txt";

if (args.Length < 2)
{
    Console.Error.WriteLine("Пути до файлов не были введены!");
    return -1;
}

var firstMatrix = new Matrix(args[0]);
var secondMatrix = new Matrix(args[1]);

var resultMatrix = Matrix.Multiplicate(firstMatrix, secondMatrix);

Matrix.Write(outputPath, resultMatrix);

Console.WriteLine($"Результат вычисления находится в файле {outputPath[12..]}");

return 0;
