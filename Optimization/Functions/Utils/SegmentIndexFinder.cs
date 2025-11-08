using Optimization.Extensions;

namespace Optimization.Functions.Utils;

public static class SegmentIndexFinder
{
    public static int Find(IVector knots, double xValue)
    {
        var knotsCount = knots.Count;
        if (xValue.EqualsWithPrecision(knots[knotsCount - 1]))
        {
            return knotsCount - 2;
        }
        
        var left = 0;
        var right = knotsCount - 2;

        while (left <= right)
        {
            var mid = (left + right) >> 1;
            if (xValue < knots[mid])
            {
                right = mid - 1;
            }
            else if (xValue >= knots[mid + 1])
            {
                left = mid + 1;
            }
            else
            {
                return mid;
            }
        }

        return Math.Max(0, Math.Min(knotsCount - 2, left));
    }
}