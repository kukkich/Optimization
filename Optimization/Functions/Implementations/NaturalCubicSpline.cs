namespace Optimization.Functions.Implementations;

public class NaturalCubicSpline : IFunction
{
    private readonly Vector _x; // узлы x, размер n
    private readonly Vector _a; // коэффициенты на интервалах, размер n-1
    private readonly Vector _b;
    private readonly Vector _c;
    private readonly Vector _d;
    private readonly int _n; // число узлов

    public NaturalCubicSpline(IVector x, IVector y)
    {
        if (x.Count != y.Count) throw new ArgumentException("x и y должны иметь одинаковую длину.");
        if (x.Count < 2) throw new ArgumentException("Нужно минимум 2 узла.");

        _n = x.Count;
        _x = new Vector(_n);
        for (int i = 0; i < _n; i++)
        {
            if (double.IsNaN(x[i]) || double.IsInfinity(x[i]) ||
                double.IsNaN(y[i]) || double.IsInfinity(y[i]))
                throw new ArgumentException("Узлы содержат NaN или Infinity.");

            if (i > 0 && !(x[i] > x[i - 1]))
                throw new ArgumentException("x должны быть строго возрастающими.");

            _x.Add(x[i]);
        }

        _a = new Vector(_n - 1);
        _b = new Vector(_n - 1);
        _c = new Vector(_n - 1);
        _d = new Vector(_n - 1);

        BuildCoefficients(x, y);
    }

    public double Value(IVector point)
    {
        if (point == null) throw new ArgumentNullException(nameof(point));
        if (point.Count != 1) throw new ArgumentException("Точка должна быть одномерной (size=1).", nameof(point));

        double xv = point[0];
        if (xv < _x[0] || xv > _x[_n - 1])
            throw new ArgumentOutOfRangeException(nameof(point), "Точка вне области определения сплайна.");

        int i = FindInterval(xv);
        double dx = xv - _x[i];
        return _a[i] + _b[i] * dx + _c[i] * dx * dx + _d[i] * dx * dx * dx;
    }

    private void BuildCoefficients(IVector x, IVector y)
    {
        int n = x.Count;
        int m = n - 1;

        var h = new Vector(m);
        for (int i = 0; i < m; i++)
        {
            double hi = x[i + 1] - x[i];
            if (!(hi > 0.0)) throw new ArgumentException("x должны быть строго возрастающими.");
            h.Add(hi);
        }

        // Вторые производные в узлах
        var M = new Vector(n);
        for (int i = 0; i < n; i++) M.Add(0.0);

        int interior = n - 2;
        if (interior > 0)
        {
            var A = new Vector(interior);
            var B = new Vector(interior);
            var C = new Vector(interior);
            var D = new Vector(interior);

            for (int i = 0; i < interior; i++)
            {
                int k = i + 1;           // глобальный индекс узла (1..n-2)
                double hk_1 = h[k - 1];  // h_{k-1}
                double hk = h[k];        // h_k

                A.Add(hk_1);
                B.Add(2.0 * (hk_1 + hk));
                C.Add(hk);

                double slopeNext = (y[k + 1] - y[k]) / hk;
                double slopePrev = (y[k] - y[k - 1]) / hk_1;
                D.Add(6.0 * (slopeNext - slopePrev));
            }

            var Mint = new Vector(interior);
            SolveTridiagonal(A, B, C, D, Mint); // решаем на внутренних узлах

            for (int i = 0; i < interior; i++)
            {
                int k = i + 1;
                M[k] = Mint[i];
            }
        }
        // M[0] = 0, M[n-1] = 0 уже выставлены (natural)

        for (int i = 0; i < m; i++)
        {
            double hi = h[i];

            _a.Add(y[i]);
            double bi = (y[i + 1] - y[i]) / hi - hi * (2.0 * M[i] + M[i + 1]) / 6.0;
            _b.Add(bi);
            _c.Add(M[i] / 2.0);
            _d.Add((M[i + 1] - M[i]) / (6.0 * hi));
        }
    }

    private int FindInterval(double xv)
    {
        // Бинарный поиск интервала [x[i], x[i+1]] для xv
        int i = 0;
        int j = _n - 1;
        // Если xv == последнему узлу, вернем последний интервал
        if (xv == _x[_n - 1]) return _n - 2;

        while (i + 1 < j)
        {
            int mid = (i + j) / 2;
            if (xv < _x[mid]) j = mid;
            else i = mid;
        }
        return i; // гарантированно 0..n-2
    }

    private static void SolveTridiagonal(IVector a, IVector b, IVector c, IVector d, IVector xOut)
    {
        int n = d.Count;
        if (a.Count != n || b.Count != n || c.Count != n)
            throw new ArgumentException("Размерности диагоналей некорректны.");
        if (n == 0)
            return;

        var cPrime = new Vector(n);
        var dPrime = new Vector(n);
        for (int i = 0; i < n; i++) { cPrime.Add(0.0); dPrime.Add(0.0); }

        double denom = b[0];
        if (denom == 0.0) throw new InvalidOperationException("Выражение вырождено (b[0] == 0).");

        cPrime[0] = (n > 1) ? c[0] / denom : 0.0;
        dPrime[0] = d[0] / denom;

        for (int i = 1; i < n; i++)
        {
            denom = b[i] - a[i] * cPrime[i - 1];
            if (denom == 0.0) throw new InvalidOperationException("Вырожденная трехдиагональная система.");
            cPrime[i] = (i < n - 1) ? c[i] / denom : 0.0;
            dPrime[i] = (d[i] - a[i] * dPrime[i - 1]) / denom;
        }

        for (int i = 0; i < n; i++) xOut.Add(0.0);
        xOut[n - 1] = dPrime[n - 1];
        for (int i = n - 2; i >= 0; i--)
            xOut[i] = dPrime[i] - cPrime[i] * xOut[i + 1];
    }
}