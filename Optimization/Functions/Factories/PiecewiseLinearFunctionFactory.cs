using Optimization.Functions.Implementations;

namespace Optimization.Functions.Factories;

public sealed class PiecewiseLinearFunctionFactory : IParametricFunction
{
    public IFunction Bind(IVector parameters)
    {
        if (parameters.Count < 4 || (parameters.Count % 2) != 0)
        {
            throw new ArgumentException("Expected even nums of parameters >= 4: (x0,y0,x1,y1,...)");
        }
        
        var n = parameters.Count / 2;
        var x = new Vector(n);
        var y = new Vector(n);

        for (var i = 0; i < n; i++)
        {
            x.Add(parameters[2 * i]);
            y.Add(parameters[2 * i + 1]);

            if (i > 0 && !(x[i] > x[i - 1]))
            {
                throw new ArgumentException("The abscissas must strictly increase: x0 < x1 < ... < xN-1");
            }
        }

        return new PiecewiseLinearFunction(x, y);
    }
}