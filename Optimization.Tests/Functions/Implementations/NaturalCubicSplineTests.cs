using Optimization.Functions;
using Optimization.Functions.Factories;
using Optimization.Functions.Implementations;

namespace Optimization.Tests.Functions.Implementations;

[TestFixture]
public class NaturalCubicSplineTests
{
    [Test]
    public void LinearData_ReproducesLine()
    {
        // y = 2x + 3
        var parameters = new Vector(8) { 0.0, 3.0, 1.0, 5.0, 2.0, 7.0, 4.0, 11.0 };
        var factory = new NaturalCubicSplineFactory();
        var f = factory.Bind(parameters);

        Assert.That(f.Value(new Vector(1) {0}), Is.EqualTo(3).Within(1e-12));
        Assert.That(f.Value(new Vector(1) {1}), Is.EqualTo(5).Within(1e-12));
        Assert.That(f.Value(new Vector(1) {2}), Is.EqualTo(7).Within(1e-12));
        Assert.That(f.Value(new Vector(1) {4}), Is.EqualTo(11).Within(1e-12));

        Assert.That(f.Value(new Vector(1) {0.5}), Is.EqualTo(2 * 0.5 + 3).Within(1e-12));
        Assert.That(f.Value(new Vector(1) {3}), Is.EqualTo(2 * 3 + 3).Within(1e-12));
    }
    
    [Test]
    public void Gradient_Linear_ReproducesSlopeEverywhere()
    {
        // y = 2x + 3 => S'(x) = 2
        var parameters = new Vector(8) { 0.0, 3.0, 1.0, 5.0, 2.0, 7.0, 4.0, 11.0 };
        var factory = new NaturalCubicSplineFactory();
        var f = factory.Bind(parameters) as IDifferentiableFunction; 

        double[] testX = { 0, 0.5, 1, 1.5, 3, 4 };
        foreach (var xv in testX)
        {
            var g = f.Gradient(new Vector(1) {xv});
            Assert.That(g[0], Is.EqualTo(2.0).Within(1e-12));
        }
    }
}