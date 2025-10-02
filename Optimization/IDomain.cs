namespace Optimization;

public interface IDomain
{
    public int Dimension { get; }
    public IReadOnlyList<IVector> Nodes { get; }
}