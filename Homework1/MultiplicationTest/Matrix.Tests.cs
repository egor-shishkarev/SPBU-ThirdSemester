namespace Matrices.Tests;

public class MatricesTests
{
    private static bool AreMatricesIdentical(Matrix firstMatrix, Matrix secondMatrix)
    {
        if (firstMatrix.Width != secondMatrix.Width || firstMatrix.Height != secondMatrix.Height)
        {
            return false;
        }
        for (int i = 0; i < firstMatrix.Height; ++i)
        {
            for (int j = 0; j < firstMatrix.Width; ++j)
            {
                if (firstMatrix.Content[i][j] != secondMatrix.Content[i][j])
                {
                    return false;
                }
            }
        }
        return true;
    }

    [TestCase(null)]
    public void NullPathShouldThrowExceptionTest(string path)
    {
        Assert.Throws<ArgumentNullException>(() => new Matrix(path));
    }

    [TestCase("NoSuchFile.txt")]
    public void FileNotFoundShouldThrowExceptionTest(string path)
    {
        Assert.Throws<FileNotFoundException>(() => new Matrix(path));
    }

    [TestCase("Empty.txt")]
    public void EmptyFileShouldThrowExceptionTest(string path)
    {
        Assert.Throws<ArgumentException>(() => new Matrix(path));
    }

    [TestCase("NotSupportedSymbols.txt")]
    public void SymbolsExceptNumbersShouldThrowExceptionTest(string path)
    {
        Assert.Throws<FormatException>(() => new Matrix(path));
    }

    [TestCase("WrongFormatOfMatrix.txt")]
    public void WrongFormatOfMatrixShouldThrowExceptionTest(string path)
    {
        Assert.Throws<FormatException>(() => new Matrix(path));
    }

    [TestCase("WrongCountOfElementsInRow.txt")]
    public void WrongCountOfElementsInRowsShouldThrowExceptionTest(string path)
    {
        Assert.Throws<ArgumentException>(() => new Matrix(path));
    }

    [TestCase("CommonMatrix.txt")]
    public void ReadMatrixShouldWorkCorrectlyTest(string path)
    {
        var matrix = new Matrix(path);
        List<List<int>> matrixContent = new () { new List<int>() { 1, 2, 3, 4 }, 
            new List<int>() { 5, 6, 7, 8 }, new List<int>() { 9, 10, 11, 12 }, new List<int>() { 13, 14, 15, 16 } };
        Assert.That(matrix.Content, Is.EqualTo(matrixContent));
    }

    [TestCase("Matrix3x3.txt", "Matrix2x3.txt")]
    public void MatricesWithDifferentCountOfColumnsAndRowsShouldThrowExceptionTest(string firstPath, string secondPath)
    {
        var firstMatrix = new Matrix(firstPath);
        var secondMatrix = new Matrix(secondPath);
        Assert.Throws<NotSupportedException>(() => Matrix.Multiplicate(firstMatrix, secondMatrix));
    }

    [TestCase("MatrixWithFractionalNumbers.txt")]
    public void MatrixWithFractionalNumbresShouldThrowExceptionTest(string path)
    {
        Assert.Throws<FormatException>(() => new Matrix(path));
    }

    [TestCase("Matrix2x3.txt", "Matrix3x3.txt")]
    public void MultiplicationWithoutMultithreadingShouldReturnExpectedResultTest(string firstPath, string secondPath)
    {
        var firstMatrix = new Matrix(firstPath);
        var secondMatrix = new Matrix(secondPath);
        List<List<int>> resultMatrix = new() { new List<int>() { 12, 12, 12 }, new List<int>() { 66, 81, 96 } };
        Assert.That(Matrix.Multiplicate(firstMatrix, secondMatrix).Content, Is.EqualTo(resultMatrix));
    }

    [TestCase("Matrix2x3.txt", "Matrix3x3.txt")]
    [TestCase("BigMatrix1.txt", "BigMatrix2.txt")]
    [TestCase("CommonMatrix.txt", "CommonMatrix.txt")]
    public void MultiplicationWithMultithreadingAndWithoutShouldReturnSameResultTest(string firstPath, string secondPath)
    {
        var firstMatrix = new Matrix(firstPath);
        var secondMatrix = new Matrix(secondPath);
        var matrixWithoutMultithreading = Matrix.Multiplicate(firstMatrix, secondMatrix);
        var matrixWithMultithreading = Matrix.MultiplicateWithMultithreading(firstMatrix, secondMatrix);
        Assert.That(AreMatricesIdentical(matrixWithMultithreading, matrixWithoutMultithreading), Is.True);
    }
}
