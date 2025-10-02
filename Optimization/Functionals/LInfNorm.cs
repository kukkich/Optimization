using Optimization.Functions;

namespace Optimization.Functionals;

public class LInfNorm : IFunctional
{
    private readonly IDomain _domain;
    private readonly double[] _y;

    public LInfNorm(IDomain domain, IFunction originalFunction)
    {
        _y = new double[domain.Nodes.Count];

        for (var i = 0; i < _y.Length; i++)
        {
            _y[i] = originalFunction.Value(domain.Nodes[i]);
        }
        
        _domain = domain;
    }

    public double Value(IFunction function)
    {
        var max = _domain.Nodes
            .Select((node, index) => function.Value(node) - _y[index])
            .Max(Math.Abs);

        return max;
    }
}