namespace Optimization;

public interface IMatrix : IList<IList<double>>;

public class Matrix : List<IList<double>>, IMatrix
{
    public Matrix(int n) : this(n, n)
    {
    }

    public Matrix(int n, int m) : base(n)
    {
        for (var i = 0; i < n; i++)
        {
            this[i] = new List<double>(m);
        }
    }
}

public static class MatrixExtensions
{
    public static IMatrix MultiplyTransposeOn(this IMatrix self)
    {
        var resultMatrix = new Matrix(self.Count, self[0].Count);

        for (var i = 0; i < self.Count; i++)
        {
            for (var j = 0; j < self[i].Count; j++)
            {
                var sum = 0d;
                for (var k = 0; k < self.Count; k++)
                {
                    sum += self[k][i] * self[k][j];
                }

                resultMatrix[i][j] = sum;
            }
        }

        return resultMatrix;
    }

    public static IVector MultiplyTransposeOn(this IMatrix self, IVector other)
    {
        var resultVector = new Vector(self[0].Count);

        for (var i = 0; i < self.Count; i++)
        {
            var sum = 0d;

            for (var j = 0; j < self[i].Count; j++)
            {
                sum += self[j][i] * other[j];
            }

            resultVector[i] = sum;
        }

        return resultVector;
    }
}

