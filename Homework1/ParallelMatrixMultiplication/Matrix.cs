namespace Matrices;

using System.Text;

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
    public List<List<int>> Content;

    /// <summary>
    /// Constructor for empty matrix.
    /// </summary>
    public Matrix()
    {
        Width = 0;
        Height = 0;
        Content = new List<List<int>>();
    }

    /// <summary>
    /// Contructor for matrix with given width and height.
    /// </summary>
    public Matrix(int width, int height)
    {
        Width = width;
        Height = height;
        Content = new List<List<int>>();
    }

    /// <summary>
    /// Constructor for matrix from file.
    /// </summary>
    /// <param name="path"></param>
    public Matrix(string path)
    {
        Content = Read(path);
        Check();
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
        var resultMatrix = new Matrix(firstMatrix.Width, secondMatrix.Height);
        for (int i = 0; i < firstMatrix.Height; ++i)
        {
            var currentRow = new List<int>();
            for (int j = 0; j < secondMatrix.Width; ++j)
            {
                var currentNumber = 0;
                for (int k = 0; k < firstMatrix.Width; ++k)
                {
                    currentNumber += firstMatrix.Content[i][k] * secondMatrix.Content[k][j];
                }
                currentRow.Add(currentNumber);
            }
            resultMatrix.Content.AddRange(new List<List<int>>() { currentRow });
        }
        return resultMatrix;
    }

    //public static Matrix MultiplicateWithMultithreading(Matrix firstMatrix, Matrix secondMatrix)
    //{

    //}

    /// <summary>
    /// Method, which reads matrix from file.
    /// </summary>
    /// <param name="path">Path to file, where the matrix is located.</param>
    /// <returns>Matrix as a list of lists</returns>
    /// <exception cref="ArgumentNullException">Path was null.</exception>
    /// <exception cref="FileNotFoundException">File with such path doesn't exists.</exception>
    /// <exception cref="ArgumentException">File was empty.</exception>
    /// <exception cref="FormatException">Wrong format of matrix.</exception>
    private static List<List<int>> Read(string path)
    {
        if (path == null)
        {
            throw new ArgumentNullException(path, "Путь до файла не был введен!");
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
        List<List<int>> matrixInt = new();
        foreach (var substring in matrix)
        {
            try
            {
                matrixInt.Add(substring.Split(' ').Select(Int32.Parse).ToList());
            }
            catch (System.FormatException)
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
    private void Check()
    {
        int currentLengthOLine = Content[0].Count;
        foreach(var line in Content)
        {
            if (line.Count != currentLengthOLine)
            {
                throw new FormatException("Количество элементов в строках матрицы не одинаковое!");
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
       var matrixToWrite = ToStringArray(matrix);
       File.WriteAllText("../../../../ResultMatrix.txt", matrixToWrite);
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
            matrixInStringArray.Append("\n");
        }
        return matrixInStringArray.ToString();
    }
}
