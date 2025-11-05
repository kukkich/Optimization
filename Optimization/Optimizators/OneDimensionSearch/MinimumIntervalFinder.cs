namespace Optimization.Optimizators.OneDimensionSearch;

public interface IMinimumIntervalFinder
{
    public Interval FindMinimumInterval(double startPoint, Func<double, double> function);
}

public class MinimumIntervalFinder : IMinimumIntervalFinder
{
    private readonly double _minimumIntervalDelta;

    public MinimumIntervalFinder(double minimumIntervalDelta = 1e-6)
    {
        _minimumIntervalDelta = minimumIntervalDelta;
    }

    public Interval FindMinimumInterval(double startPoint, Func<double, double> function)
    {
        var xPrev = startPoint;
        var xNext = 0d;
        var h = 0d;

        var fValue = function(xPrev);

        if (fValue > function(xPrev + _minimumIntervalDelta))
        {
            xNext = xPrev + _minimumIntervalDelta;
            h = _minimumIntervalDelta;
        }
        else if (fValue > function(xPrev - _minimumIntervalDelta))
        {
            xNext = xPrev - _minimumIntervalDelta;
            h = -_minimumIntervalDelta;
        }

        xPrev = xNext;
        h *= 2;
        xNext = xPrev + h;

        var minimumInterval = new Interval(xPrev, xNext);

        for (; function(xPrev) > function(xNext);)
        {
            xPrev = xNext;
            h *= 2;
            xNext = xPrev + h;

            minimumInterval.Left = xPrev;
            minimumInterval.Right = xNext;
        }

        minimumInterval.Left = xPrev - h / 2;

        return minimumInterval;
    }
}

public class Interval(double left, double right)
{
    public double Left { get; set; } = left;
    public double Right { get; set; } = right;
}