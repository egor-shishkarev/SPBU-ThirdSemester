using BenchmarkDotNet.Attributes;

namespace Matrices;

public class BenchmarkMatrix
{
    [Params(10, 100, 200, 300, 400)]
    public int size;

    [Benchmark]
    public void ParallelMatrixMultiplication()
    {
        var unitMatrix = Matrix.Generate(size, size, 1, 2);
        Matrix.MultiplicateWithMultithreading(unitMatrix, unitMatrix);
    }

    [Benchmark]
    public void MatrixMultiplication()
    {
        var unitMatrix = Matrix.Generate(size, size, 1, 2);
        Matrix.Multiplicate(unitMatrix, unitMatrix);
    }
}