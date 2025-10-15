namespace Optimization;

public interface IVector : IList<double>;

public class Vector(int dimension) : List<double>(dimension), IVector;

public static class VectorExtensions
{
    public static IVector Sum(this IVector self, IVector other)
    {
        for (var i = 0; i < self.Count; i++)
        {
            self[i] += other[i];
        }

        return self;
    }

    public static IVector Subtract(this IVector self, IVector other)
    {
        for (var i = 0; i < self.Count; i++)
        {
            self[i] -= other[i];
        }

        return self;
    }

    public static IVector Multiply(this IVector self, double number)
    {
        for (var i = 0; i < self.Count; i++)
        {
            self[i] = number * self[i];
        }

        return self;
    }

    public static IVector Negate(this IVector self)
    {
        for (var i = 0; i < self.Count; i++)
        {
            self[i] = -self[i];
        }

        return self;
    }

    public static IVector Clone(this IVector self)
    {
        var newVector = new Vector(self.Count);

        for (var i = 0; i < newVector.Count; i++)
        {
            newVector[i] = self[i];
        }

        return newVector;
    }

    public static double ScalarProduct(this IVector self, IVector other)
    {
        var scalarProduct = 0d;

        for (var i = 0; i < self.Count; i++)
        {
            scalarProduct += self[i] * other[i];
        }

        return scalarProduct;
    }

    public static double ScalarProduct(this IVector self) => self.ScalarProduct(self);
    public static double Norm(this IVector self) => Math.Sqrt(self.ScalarProduct(self));
}