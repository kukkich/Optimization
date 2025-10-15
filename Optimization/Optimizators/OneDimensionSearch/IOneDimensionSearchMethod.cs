namespace Optimization.Optimizators.OneDimensionSearch;

public interface IOneDimensionSearchMethod
{
    public double SearchMin(Func<double, double> function, double precision = 1e-7);
}