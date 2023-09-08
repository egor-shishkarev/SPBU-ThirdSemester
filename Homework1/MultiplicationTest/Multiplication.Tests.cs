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

/* ��� �������� � ����� - 
�������� �� ���������� ���������� ����� (��� �� ����, ��� � ��� ���-�� ����, ��� ��� ������������� �������)
�������� �� ������������ �������� ��������� ������
������� ��������� �� ������:
1)��� ���� - ����������
2)��� ����� - ����������
3)������ ���� - ����������
4)���-�� ����� ���� - ����������
5)�� ���������� ������ ������� - ����������
6)�� ���������� ���-�� ��������� - ����������
7)������ �������� �� ���� �������
8)���������� �� ��������� ������ � ������� ���-��� �������� � �����
9)������� � �������� ������� - ����������
10)������� ��������� ��� ���������������
11)�������� ��������������� �� ������� ��������
*/
