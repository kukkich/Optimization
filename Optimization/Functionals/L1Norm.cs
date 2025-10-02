using Optimization.Functions;

namespace Optimization.Functionals;

/// <summary>
/// Sum(|f(x_i) - y_i|)
/// </summary>
public class L1Norm : IDifferentiableFunctional
{
    private readonly IDomain _domain;
    private readonly double[] _y;

    public L1Norm(IDomain domain, IFunction originalFunction)
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
        var sum = _domain.Nodes
            .Select((node, index) => Math.Abs(function.Value(node) - _y[index]))
            .Sum();

        return sum;
    }

    public IVector Gradient(IFunction function)
    {
        var gradient = new Vector(_domain.Dimension);
        
        for (var i = 0; i < _domain.Nodes.Count; i++)
        {
            var node = _domain.Nodes[i];
            var r = function.Value(node) - _y[i];
            gradient[i] = Math.Sign(r);
        }
        
        return gradient;
    }
}