using Optimization.Functions;

namespace Optimization.Functionals;

/// <summary>
/// Sum((f(x_i) - y_i)^2)
/// </summary>
public class L2Norm : IDifferentiableFunctional, ILeastSquaresFunctional
{
    private readonly IDomain _domain;
    private readonly double[] _y;

    public L2Norm(IDomain domain, IFunction originalFunction)
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
            .Select((node, index) => function.Value(node) - _y[index])
            .Sum(x => x * x);

        return sum;
    }

    public IVector Gradient(IFunction function)
    {
        var gradient = new Vector(_domain.Dimension);
        
        for (var i = 0; i < _domain.Nodes.Count; i++)
        {
            var node = _domain.Nodes[i];
            gradient[i] = 2 * function.Value(node) - _y[i];
        }
        
        return gradient;
    }

    public IVector Residual(IFunction function) => Gradient(function);

    public IMatrix Jacobian(IFunction function)
    {
        // мб тут нужно например IParametricFunction 
        throw new NotImplementedException("Не очень понятно как считать: нужно n функций, а у нас только 1 function");
    }
}