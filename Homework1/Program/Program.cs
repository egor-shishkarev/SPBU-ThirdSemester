using Matrices;

Console.Write("Введите пути к файлам, содержащие матрицы для умножения\nВведите первый путь => ");
var firstPath = Console.ReadLine();
Console.Write("Введите второй путь => ");
var secondPath = Console.ReadLine();

var firstMatrix = new Matrix(firstPath!);
var secondMatrix = new Matrix(secondPath!);

firstMatrix.PrintMatrix();
secondMatrix.PrintMatrix();

var resultMatrix = Matrix.Multiplicate(firstMatrix, secondMatrix);
resultMatrix.PrintMatrix();

return 0;