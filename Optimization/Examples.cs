using Optimization.Functionals;
using Optimization.Functions;
using Optimization.Optimizators;

namespace Optimization;

public class LineFunction : IParametricFunction
{
    class InternalLineFunction : IFunction
    {
        public double A { get; init; }
        public double B { get; init; }
        public double Value(IVector point) => A * point[0] + B;
    }

    public IFunction Bind(IVector parameters) => new InternalLineFunction { A = parameters[0], B = parameters[1] };
}

public class MyFunctional : IFunctional
{
    public List<(double x, double y)> Points;

    public double Value(IFunction function)
    {
        return Points
            .Select(point => new { point, param = new Vector { point.x } })
            .Select(t => function.Value(t.param) - t.point.y)
            .Select(s => s * s)
            .Sum();
    }
}

class MinimizerMonteCarlo : IOptimizator
{
    public int MaxIter = 100000;

    public IVector Minimize(IFunctional objective, IParametricFunction function, IVector initialParameters,
        IVector minimumParameters = null, IVector maximumParameters = null)
    {
        var param = new Vector();
        var minparam = new Vector();
        param.AddRange(initialParameters);
        minparam.AddRange(initialParameters);
        var fun = function.Bind(param);
        var currentmin = objective.Value(fun);
        var rand = new Random(0);
        for (var i = 0; i < MaxIter; i++)
        {
            for (var j = 0; j < param.Count; j++) 
                param[j] = rand.NextDouble();
            var f = objective.Value(function.Bind(param));
            if (!(f < currentmin))
                continue;
            currentmin = f;
            for (var j = 0; j < param.Count; j++) 
                minparam[j] = param[j];
        }

        return minparam;
    }
}

class Example
{
    static void Run()
    {
        var optimizer = new MinimizerMonteCarlo();
        var initial = new Vector
        {
            1,
            1
        };
        var n = int.Parse(Console.ReadLine());
        List<(double x, double y)> points = new();
        for (var i = 0; i < n; i++)
        {
            var str = Console.ReadLine().Split();
            points.Add((double.Parse(str[0]), double.Parse(str[1])));
        }

        var functinal = new MyFunctional { Points = points };
        var fun = new LineFunction();

        var res = optimizer.Minimize(functinal, fun, initial);
        Console.WriteLine($"a={res[0]},b={res[1]}");
    }
}