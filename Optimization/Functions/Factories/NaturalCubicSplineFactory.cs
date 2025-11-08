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
        
        var knotCount = parameters.Count / 2;
        var knots = new Vector(knotCount);
        var values = new Vector(knotCount);

        for (var i = 0; i < knotCount; i++)
        {
            var knot = parameters[2 * i];
            var value = parameters[2 * i + 1];
            
            knots.Add(knot);
            values.Add(value);

            if (i > 0 && !(knots[i] > knots[i - 1]))
            {
                throw new ArgumentException("Абсциссы узлов x должны быть строго возрастающими.");
            }
        }

        return new NaturalCubicSpline(knots, values);
    }
}