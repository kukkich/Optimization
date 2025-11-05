namespace Optimization.Optimizators.EquationSystem;

public interface ISLAESolver
{
    public IVector Solve(IMatrix leftPart, IVector rightPart);
}