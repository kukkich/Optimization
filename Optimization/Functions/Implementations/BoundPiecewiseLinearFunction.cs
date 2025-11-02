namespace Optimization.Functions.Implementations;

public class BoundPiecewiseLinearFunction : IDifferentiableFunction
{
    private readonly double[] _x;
    private readonly double[] _y;

    public BoundPiecewiseLinearFunction(double[] x, double[] y)
    {
        _x = (double[])x.Clone();
        _y = (double[])y.Clone();
    }

    public double Value(IVector point)
    {
        if (point == null) throw new ArgumentNullException(nameof(point));
        if (point.Count != 1) throw new ArgumentException("Piecewise-linear function expects 1D point (x).");
        double xv = point[0];

        int i = FindIntervalOrExtrapIndex(_x, xv, out bool leftExtrap, out bool rightExtrap);

        if (leftExtrap)
        {
            // Линейная экстраполяция слева по первому отрезку
            double h = _x[1] - _x[0];
            double slope = (_y[1] - _y[0]) / h;
            return _y[0] + slope * (xv - _x[0]);
        }
        if (rightExtrap)
        {
            // Линейная экстраполяция справа по последнему отрезку
            int n1 = _x.Length - 1;
            double h = _x[n1] - _x[n1 - 1];
            double slope = (_y[n1] - _y[n1 - 1]) / h;
            return _y[n1] + slope * (xv - _x[n1]);
        }

        // Интерполяция на отрезке [x_i, x_{i+1}]
        double hseg = _x[i + 1] - _x[i];
        double t = (xv - _x[i]) / hseg;
        return (1 - t) * _y[i] + t * _y[i + 1];
    }

    public IVector Gradient(IVector point)
    {
        if (point == null) throw new ArgumentNullException(nameof(point));
        if (point.Count != 1) throw new ArgumentException("Piecewise-linear function expects 1D point (x).");
        double xv = point[0];

        int i = FindIntervalOrExtrapIndex(_x, xv, out bool leftExtrap, out bool rightExtrap);

        double slope;
        if (leftExtrap)
        {
            double h = _x[1] - _x[0];
            slope = (_y[1] - _y[0]) / h;
        }
        else if (rightExtrap)
        {
            int n1 = _x.Length - 1;
            double h = _x[n1] - _x[n1 - 1];
            slope = (_y[n1] - _y[n1 - 1]) / h;
        }
        else
        {
            double hseg = _x[i + 1] - _x[i];
            slope = (_y[i + 1] - _y[i]) / hseg;

            // В точности в узле производная неоднозначна. Примем левый наклон, кроме самого первого узла.
            if (Math.Abs(xv - _x[i + 1]) < 1e-15 && i + 1 < _x.Length - 1)
            {
                double h2 = _x[i + 2] - _x[i + 1];
                slope = (_y[i + 2] - _y[i + 1]) / h2;
            }
        }

        return new Vector(1) { slope };
    }

    // Возвращает индекс i такого, что x[i] <= xv < x[i+1], если внутри
    // Если левее — i = 0 с флагом leftExtrap; если правее — i = n-2 с флагом rightExtrap.
    private static int FindIntervalOrExtrapIndex(double[] x, double xv, out bool leftExtrap, out bool rightExtrap)
    {
        int n = x.Length;
        leftExtrap = false; rightExtrap = false;

        if (xv < x[0])
        {
            leftExtrap = true;
            return 0;
        }
        if (xv > x[n - 1])
        {
            rightExtrap = true;
            return n - 2;
        }

        // Бинарный поиск
        int lo = 0, hi = n - 1;
        while (hi - lo > 1)
        {
            int mid = (lo + hi) >> 1;
            if (x[mid] <= xv) lo = mid; else hi = mid;
        }
        return lo;
    }
}