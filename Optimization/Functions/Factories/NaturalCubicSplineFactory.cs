using Optimization.Functions.Implementations;

namespace Optimization.Functions.Factories;

public class NaturalCubicSplineFactory : IParametricFunction
{
    public IFunction Bind(IVector parameters)
    {
        if (parameters.Count < 4 || (parameters.Count % 2) != 0)
        {
            throw new ArgumentException("Expected even num of parameters >= 4: x0, y0, x1, y1, ...");
        }
        
        var n = parameters.Count / 2;
        var x = new Vector(n);
        var y = new Vector(n);

        for (var i = 0; i < n; i++)
        {
            var xi = parameters[2 * i];
            var yi = parameters[2 * i + 1];
            
            x.Add(xi);
            y.Add(yi);

            if (i > 0 && !(x[i] > x[i - 1]))
            {
                throw new ArgumentException("Абсциссы узлов x должны быть строго возрастающими.");
            }
        }

        return new NaturalCubicSpline(x, y);
    }
}