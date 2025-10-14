using Optimization.Functionals;
using Optimization.Functions;
using System;
using System.Drawing;

namespace Optimization.Optimizators;

public class SimulatedAnnealing : IOptimizator
{
    private readonly Random _random;
    private double _initialTemperature;
    private double _minTemperature;

    public SimulatedAnnealing() : this(1, 0.01) 
    {
    }

    public SimulatedAnnealing(double initialTemperature, double minTemperature)
    {
        _random = new Random();
        _initialTemperature = initialTemperature;
        _minTemperature = minTemperature;
    }

    public IVector Minimize(IFunctional objective, IParametricFunction function, IVector initialParameters,
        IVector minimumParameters = default, IVector maximumParameters = default)
    {
        var targetParameters = initialParameters.Clone();
        var targetFunctionalValue = objective.Value(function.Bind(initialParameters));

        var currentTemperature = _initialTemperature;

        var i = 1;

        while (currentTemperature > _minTemperature)
        {
            var nextParameters = GenerateNextParameters(initialParameters, _initialTemperature, minimumParameters, maximumParameters);
            var nextFunctionalValue = objective.Value(function.Bind(nextParameters));

            if (nextFunctionalValue < targetFunctionalValue || Math.Exp(-(nextFunctionalValue - targetFunctionalValue) / currentTemperature) > _random.NextDouble())
            {
                targetParameters = nextParameters;
                targetFunctionalValue = nextFunctionalValue;

                if (minimumParameters != default && maximumParameters != default)
                {
                    currentTemperature = ChangeTemperatureForUltraFastAnnealing(i, initialParameters.Count);
                }
                else
                {
                    currentTemperature = ChangeTemperatureForCauchyAnnealing(i, initialParameters.Count);
                }

                i++;
            }
        }

        return targetParameters;
    }

    private IVector GenerateNextParameters(IVector point, double currentTemperature, IVector minimumParameters = default,
        IVector maximumParameters = default)
    {
        var nextPoint = new Vector(point.Count);

        if (minimumParameters != default && maximumParameters != default)
        {
            for (var i = 0; i < nextPoint.Count; i++)
            {
                var alpha = _random.NextDouble();
                var z = Math.Sign(alpha - 1 / 2d) * currentTemperature *
                        (Math.Pow(1 + 1 / currentTemperature, Math.Abs(2 * alpha - 1)) - 1);

                nextPoint[i] = point[i] + (maximumParameters[i] - minimumParameters[i]) * z;
            }
        }
        else
        {
            for (var i = 0; i < nextPoint.Count; i++)
            {
                //Math.Tan(Math.PI * (_random.NextDouble() - 0.5)) - распределение Коши
                nextPoint[i] = point[i] + currentTemperature * Math.Tan(Math.PI * (_random.NextDouble() - 0.5));
            }
        }

        return nextPoint;
    }

    private double ChangeTemperatureForCauchyAnnealing(int iterationNumber, int parameterCount) => _initialTemperature / Math.Pow(iterationNumber, 1d / parameterCount);
    private double ChangeTemperatureForUltraFastAnnealing(int iterationNumber, int parameterCount) => _initialTemperature * Math.Exp(Math.Pow(iterationNumber, 1d / parameterCount));
}