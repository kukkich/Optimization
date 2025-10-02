namespace Optimization;

public interface IVector : IList<double>;

public class Vector(int dimension) : List<double>(dimension), IVector;