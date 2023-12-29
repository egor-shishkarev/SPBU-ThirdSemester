namespace Matrices;

using System;
using System.Text;
using System.Threading;

/// <summary>
/// Class of mathematical object - matrix.
/// </summary>
public class Matrix
{
    /// <summary>
    /// Count of elements in row.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Count of elements in column.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Representation of matrix.
    /// </summary>
    public List<List<long>> Content;

    /// <summary>
    /// Constructor for empty matrix.
    /// </summary>
    public Matrix()
    {
        Width = 0;
        Height = 0;
        Content = new List<List<long>>();
    }

    /// <summary>
    /// Contructor for matrix with given width and height.
    /// </summary>
    public Matrix(int height, int width)
    {
        Width = width;
        Height = height;
        Content = new List<List<long>>();
        for (int i = 0; i < Height; ++i)
        {
            var currentRow = new List<long>();
            for (int j = 0; j < Width; ++j)
            {
                currentRow.Add(0);
            }
            Content.Add(currentRow);
        }
    }

    /// <summary>
    /// Constructor for matrix from file.
    /// </summary>
    /// <param name="path">Path to the file, containing the matrix.</param>
    public Matrix(string path)
    {
        Content = Read(path);
        CheckRows();
        Width = Content[0].Count;
        Height = Content.Count;
    }

    /// <summary>
    /// Method, which multiplicate two matrix.
    /// </summary>
    /// <param name="firstMatrix">First matrix in multiplication.</param>
    /// <param name="secondMatrix">Second matrix in multiplication.</param>
    /// <returns>Result of multiplication - matrix.</returns>
    /// <exception cref="NotSupportedException">Inappropriate matrix sizes for multiplication.</exception>
    public static Matrix Multiplicate(Matrix firstMatrix, Matrix secondMatrix)
    {
        if (firstMatrix.Width != secondMatrix.Height)
        {
            throw new NotSupportedException("Нельзя умножать матрицы, где количество элементов строки первой матрицы, " +
                "отличается от количества элементов столбца второй матрицы!");
        }
        var resultMatrix = new Matrix(firstMatrix.Height, secondMatrix.Width);
        for (int i = 0; i < firstMatrix.Height; ++i)
        {
            for (int j = 0; j < secondMatrix.Width; ++j)
            {
                resultMatrix.Content[i][j] = GetElement(firstMatrix, secondMatrix, i * firstMatrix.Width + j);
            }
        }
        return resultMatrix;
    }

    /// <summary>
    /// Method, which multiplicate two matrix, using multithreading.
    /// </summary>
    /// <param name="firstMatrix">First matrix in multiplication.</param>
    /// <param name="secondMatrix">Second matrix in multiplication.</param>
    /// <returns>Result of multiplication - matrix.</returns>
    /// <exception cref="NotSupportedException">Inappropriate matrix sizes for multiplication.</exception>
    public static Matrix MultiplicateWithMultithreading(Matrix firstMatrix, Matrix secondMatrix)
    {
        if (firstMatrix.Width != secondMatrix.Height)
        {
            throw new NotSupportedException("Нельзя умножать матрицы, где количество элементов строки первой матрицы, " +
                "отличается от количества элементов столбца второй матрицы!");
        }
        var maxCountOfThreads = Environment.ProcessorCount;
        int countOfElementsInResultMatrix = firstMatrix.Height * secondMatrix.Width;
        var countOfThreads = countOfElementsInResultMatrix >= maxCountOfThreads ? maxCountOfThreads : countOfElementsInResultMatrix;
        var threads = new Thread[countOfThreads];
        var resultMatrix = new Matrix(firstMatrix.Height, secondMatrix.Width);
        int countOfElementsForEachThread = Convert.ToInt32(Math.Ceiling((double)countOfElementsInResultMatrix / countOfThreads));

        for (var i = 0; i < threads.Length; ++i)
        {
            var localI = i;
            threads[i] = new Thread(() =>
            {
                for (var j = localI * countOfElementsForEachThread; j < (localI + 1) * countOfElementsForEachThread && j < countOfElementsInResultMatrix; ++j)
                    resultMatrix.Content[j / firstMatrix.Width][j % firstMatrix.Width] = GetElement(firstMatrix, secondMatrix, j);
            });
        }

        foreach (var thread in threads)
            thread.Start();
        foreach (var thread in threads)
            thread.Join();

        return resultMatrix;
    }

    /// <summary>
    /// Method, which reads matrix from file.
    /// </summary>
    /// <param name="path">Path to file, where the matrix is located.</param>
    /// <returns>Matrix as a list of lists</returns>
    /// <exception cref="ArgumentNullException">Path was null.</exception>
    /// <exception cref="FileNotFoundException">File with such path doesn't exists.</exception>
    /// <exception cref="ArgumentException">File was empty.</exception>
    /// <exception cref="FormatException">Wrong format of matrix.</exception>
    private static List<List<long>> Read(string path)
    {
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path), "Путь до файла не был введен!");
        }
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Файла не существует!");
        }
        var matrix = File.ReadAllLines(path);
        if (matrix == null || matrix.Length == 0)
        {
            throw new ArgumentException("Файл пуст!");
        }
        List<List<long>> matrixInt = new();
        foreach (var substring in matrix)
        {
            try
            {
                matrixInt.Add(substring.Split(' ').Select(Int64.Parse).ToList());
            }
            catch (FormatException)
            {
                throw new FormatException("Неверная запись матрицы!");
            }
        }
        return matrixInt;
    }

    /// <summary>
    /// Method, which checks the same number of elements in each row.
    /// </summary>
    /// <exception cref="FormatException">Different number of elements in rows.</exception>
    private void CheckRows()
    {
        int currentLengthOLine = Content[0].Count;
        foreach(var line in Content)
        {
            if (line.Count != currentLengthOLine)
            {
                throw new ArgumentException("Количество элементов в строках матрицы не одинаковое!");
            }
        }
    }

    /// <summary>
    /// Additional method to print matrix in console.
    /// </summary>
    public void Print()
    {
        foreach (var sublist in Content)
        {
            foreach (var number in sublist)
            {
                Console.Write($"{number} ");
            }
            Console.Write("\n");
        }
    }

    /// <summary>
    /// Method, which write martix in a specific file.
    /// </summary>
    /// <param name="path">Path to the file containing the result of the multiplication.</param>
    /// <param name="matrix">Matrix containing the result of multiplication.</param>
    public static void Write(string path, Matrix matrix)
    {
       if (string.IsNullOrEmpty(path))
       {
            throw new ArgumentNullException(nameof(path), "Путь до файла не был введен!");
       }
       var matrixToWrite = ToStringArray(matrix);
       File.WriteAllText(path, matrixToWrite);
    }

    /// <summary>
    /// Additional method to respresent matrix in a string.
    /// </summary>
    /// <param name="matrix">Matrix, which we want to represent in string.</param>
    /// <returns>Representation of matrix by string.</returns>
    private static string ToStringArray(Matrix matrix)
    {
        var matrixInStringArray = new StringBuilder();
        foreach (var row in matrix.Content)
        {
            foreach(var column in row)
            {
                matrixInStringArray.Append(column.ToString() + " ");
            }
            matrixInStringArray.Remove(matrixInStringArray.Length - 1,1);
            matrixInStringArray.Append('\n');
        }
        return matrixInStringArray.ToString();
    }

    /// <summary>
    /// Method that allows you to calculate a certain element of the matrix
    /// </summary>
    /// <param name="firstMatrix">First matrix in multiplication.</param>
    /// <param name="secondMatrix">Second matrix in multiplication.</param>
    /// <param name="number">Number of element in result matrix</param>
    /// <returns>Element with such number in result matrix.</returns>
    private static long GetElement(Matrix firstMatrix, Matrix secondMatrix, int number)
    {
        int countOfElementsInRow = firstMatrix.Width;
        int numberOfRow = number / countOfElementsInRow;
        int numberOfColumn = number % countOfElementsInRow;
        long currentElement = 0;
        for (int i = 0; i < firstMatrix.Width; ++i)
        {
            currentElement += firstMatrix.Content[numberOfRow][i] * secondMatrix.Content[i][numberOfColumn];
        }
        return currentElement;
    }

    /// <summary>
    /// Additional method to generate matrices with given parameters.
    /// </summary>
    /// <param name="width">Count of elements in row.</param>
    /// <param name="height">Count of elements in column.</param>
    /// <param name="minInt">Minimum integer number in matrix.</param>
    /// <param name="maxInt">Maximum integer number in matrix.</param>
    /// <returns>Matrix with given size and numbers.</returns>
    public static Matrix Generate(int width, int height, int minInt, int maxInt)
    {
        var random = new Random();
        var newMatrix = new Matrix(width, height);
        for (int i = 0; i < height; ++i)
        {
            for (int j = 0; j < width; ++j)
            {
                newMatrix.Content[i][j] = random.Next(minInt, maxInt);
            }
        }
        return newMatrix;
    }
}
