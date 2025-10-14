namespace Optimization;

public interface IVector : IList<double>;

public class Vector(int dimension) : List<double>(dimension), IVector;

public static class VectorExtensions
{
    public static IVector Clone(this IVector self)
    {
        var newVector = new Vector(self.Count);

        for (var i = 0; i < newVector.Count; i++)
        {
            newVector[i] = self[i];
        }

        return newVector;
    }
}