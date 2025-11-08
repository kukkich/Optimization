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
        
        var knotCount = parameters.Count / 2;
        var knots = new Vector(knotCount);
        var values = new Vector(knotCount);

        for (var i = 0; i < knotCount; i++)
        {
            knots.Add(parameters[2 * i]);
            values.Add(parameters[2 * i + 1]);

            if (i > 0 && !(knots[i] > knots[i - 1]))
            {
                throw new ArgumentException("The abscissas must strictly increase: x0 < x1 < ... < xN-1");
            }
        }

        return new PiecewiseLinearFunction(knots, values);
    }
}