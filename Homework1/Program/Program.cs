using BenchmarkDotNet.Running;
using Matrices;


const string outputPath = "../../../../ResultMatrix.txt";

// var summary = BenchmarkRunner.Run<BenchmarkMatrix>(); // - Запуск бенчмарка программы

if (args.Length < 2)
{
    Console.Error.WriteLine("Пути до файлов не были введены!");
    return -1;
}

if (args[0] == null)
{
    Console.Error.WriteLine("Первый путь не был введен!");
    return -1;
}

if (!File.Exists(args[0]))
{
    Console.Error.WriteLine("Файла с первым путем не существует!");
    return -1;
}

if (string.IsNullOrEmpty(File.ReadAllText(args[0])))
{
    Console.Error.WriteLine("Первый файл пуст!");
    return -1;
}

if (args[1] == null)
{
    Console.Error.WriteLine("Второй путь не был введен!");
    return -1;
}

if (!File.Exists(args[1]))
{
    Console.Error.WriteLine("Файла с вторым путем не существует!");
    return -1;
}

if (string.IsNullOrEmpty(File.ReadAllText(args[1])))
{
    Console.Error.WriteLine("Второй файл пуст!");
    return -1;
}

Matrix firstMatrix;
Matrix secondMatrix;

try
{
    firstMatrix = new Matrix(args[0]);
} catch (FormatException)
{
    Console.Error.WriteLine("Неподдерживаемые символы в первой матрице!");
    return -1;
} catch (ArgumentException)
{
    Console.Error.WriteLine("Различное количество символов в строках первой матрицы!");
    return -1;
}

try
{
    secondMatrix = new Matrix(args[1]);
} catch(FormatException)
{
    Console.Error.WriteLine("Неподдерживаемые символы во второй матрице!");
    return -1;
} catch(ArgumentException)
{
    Console.Error.WriteLine("Различное количество символов в строках второй матрицы!");
    return -1;
}

if (firstMatrix.Width != secondMatrix.Height)
{
    Console.Error.WriteLine("Нельзя умножать матрицы, где количество элементов строки первой матрицы, " +
        "отличается от количества элементов столбца второй матрицы!");
    return -1;
}

Matrix.Write(outputPath, Matrix.MultiplicateWithMultithreading(firstMatrix, secondMatrix));

Console.WriteLine($"Результат вычисления находится в файле {outputPath[12..]}");

return 0;
