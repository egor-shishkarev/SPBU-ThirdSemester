namespace Multiplication.Tests;

using Matrices;

public class MultiplitcationTests
{
    [TestCase(null)]
    public void NullPathShouldThrowExceptionTest(string path)
    {
        Assert.Throws<ArgumentNullException>(() => new Matrix(path));
    }

    [TestCase("../../../../TestFiles/NoSuchFile.txt")]
    public void FileNotFoundShouldThrowExceptionTest(string path)
    {
        Assert.Throws<FileNotFoundException>(() => new Matrix(path));
    }

    [TestCase("../../../../TestFiles/Empty.txt")]
    public void EmptyFileShouldThrowExceptionTest(string path)
    {
        Assert.Throws<ArgumentException>(() => new Matrix(path));
    }

    [TestCase("../../../../TestFiles/NotSupportedSymbols.txt")]
    public void SymbolsExceptNumbersShouldThrowExceptionTest(string path)
    {
        Assert.Throws<FormatException>(() => new Matrix(path));
    }

    [TestCase("../../../../TestFiles/WrongFormatOfMatrix.txt")]
    public void WrongFormatOfMatrixShouldThrowExceptionTest(string path)
    {
        Assert.Throws<FormatException>(() => new Matrix(path));
    }

    [TestCase("../../../../TestFiles/WrongCountOfElementsInRow.txt")]
    public void WrongCountOfElementsInRowsShouldThrowExceptionTest(string path)
    {
        Assert.Throws<FormatException>(() => new Matrix(path));
    }

    [TestCase("../../../../TestFiles/CommonMatrix.txt")]
    public void ReadMatrixShouldWorkCorrectlyTest(string path)
    {
        var matrix = new Matrix(path);
        List<List<int>> matrixContent = new () { new List<int>() { 1, 2, 3, 4 }, 
            new List<int>() { 5, 6, 7, 8 }, new List<int>() { 9, 10, 11, 12 }, new List<int>() { 13, 14, 15, 16 } };
        Assert.That(matrix.Content, Is.EqualTo(matrixContent));
    }

    [TestCase("../../../../TestFiles/Matrix3x3.txt", "../../../../TestFiles/Matrix2x3.txt")]
    public void MatricesWithDifferentCountOfColumnsAndRowsShouldThrowExceptionTest(string firstPath, string secondPath)
    {
        var firstMatrix = new Matrix(firstPath);
        var secondMatrix = new Matrix(secondPath);
        Assert.Throws<NotSupportedException>(() => Matrix.Multiplicate(firstMatrix, secondMatrix));
    }

    [TestCase("../../../../TestFiles/MatrixWithFractionalNumbers.txt")]
    public void MatrixWithFractionalNumbresShouldThrowExceptionTest(string path)
    {
        Assert.Throws<FormatException>(() => new Matrix(path));
    }

    [TestCase("../../../../TestFiles/Matrix2x3.txt", "../../../../TestFiles/Matrix3x3.txt")]
    public void MultiplicationWithoutMultithreadingShouldReturnExpectedResultTest(string firstPath, string secondPath)
    {
        var firstMatrix = new Matrix(firstPath);
        var secondMatrix = new Matrix(secondPath);
        List<List<int>> resultMatrix = new() { new List<int>() { 12, 12, 12 }, new List<int>() { 66, 81, 96 } };
        Assert.That(Matrix.Multiplicate(firstMatrix, secondMatrix).Content, Is.EqualTo(resultMatrix));
    }
}

/* Что добавить в тесты - 
Проверка на корректное содержание файла (Что он есть, что в нем что-то есть, что там действительно матрица)
Проверка на корректность подсчета умножения матриц
Поехали конкретно по тестам:
1)Нет пути - исключение
2)Нет файла - исключение
3)Пустой файл - исключение
4)Что-то кроме цифр - исключение
5)Не правильный формат матрицы - исключение
6)Не правильное кол-во элементов - исключение
7)Просто проверка на ввод матрицы
8)Исключение на умножение матриц с разнымы кол-вом столбцов и строк
9)Матрица с дробными числами - исключение
10)Обычное умножение без многопоточности
11)Проверка многопоточности на больших матрицах
*/
