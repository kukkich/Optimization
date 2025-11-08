using Optimization.Functions;
using Optimization.Functions.Factories;
using Optimization.Functions.Implementations;

namespace Optimization.Tests.Functions.Implementations;

[TestFixture]
public class PiecewiseLinearFunctionTests
{
    [Test]
    public void Value_And_Gradient_Work_With_ThreeSegments()
    {
        // (0,0)-(1,1): slope = 1
        // (1,1)-(4,5): slope = 4/3
        // (4,5)-(5,6): slope = 1
        var parameters = new Vector(8) { 0.0, 0.0, 1.0, 1.0, 4.0, 5.0, 5.0, 6.0 };
        var factory = new PiecewiseLinearFunctionFactory();
        var f = factory.Bind(parameters) as IDifferentiableFunction;

        Assert.That(f.Value(new Vector(1) { 0.0 }), Is.EqualTo(0.0 + 1.0 * 0.0).Within(1e-12)); // 0.0...
        Assert.That(f.Value(new Vector(1) { 0.5 }), Is.EqualTo(0.0 + 1.0 * 0.5).Within(1e-12)); // 0.5...
        Assert.That(f.Value(new Vector(1) { 1.0 }), Is.EqualTo(1.0 + (4.0 / 3.0) * 0.0).Within(1e-12)); // 1.0...
        Assert.That(f.Value(new Vector(1) { 1.5 }), Is.EqualTo(1.0 + (4.0 / 3.0) * 0.5).Within(1e-12)); // 1.666...
        Assert.That(f.Value(new Vector(1) { 4.0 }), Is.EqualTo(5.0 + 1.0 * 0.0).Within(1e-12)); // 5.0
        Assert.That(f.Value(new Vector(1) { 4.5 }), Is.EqualTo(5.0 + 1.0 * 0.5).Within(1e-12)); // 5.5
        Assert.That(f.Value(new Vector(1) { 5.0 }), Is.EqualTo(6.0 + 1.0 * 0.0).Within(1e-12)); // 6.0
    }

    [Test]
    public void Gradient_Interior_ReturnsSlope_OfCorrespondingSegment()
    {
        // (0,0)-(1,2): slope = 2
        // (1,2)-(2,3): slope = 1
        var parameters = new Vector(6) { 0.0, 0.0, 1.0, 2.0, 2.0, 3.0 };
        var factory = new PiecewiseLinearFunctionFactory();
        var f = factory.Bind(parameters) as IDifferentiableFunction;

        var g1 = f.Gradient(new Vector(1) { 0.25 });
        Assert.That(g1.Count, Is.EqualTo(1));
        Assert.That(g1[0], Is.EqualTo(2.0).Within(1e-12));

        var g2 = f.Gradient(new Vector(1) { 1.5 });
        Assert.That(g2.Count, Is.EqualTo(1));
        Assert.That(g2[0], Is.EqualTo(1.0).Within(1e-12));
    }

    [Test]
    public void Gradient_Throws_AtBoundaries()
    {
        // (0,0)-(1,2): slope = 2
        // (1,2)-(3,6): slope = 2
        var parameters = new Vector(6) { 0.0, 0.0, 1.0, 2.0, 3.0, 6.0 };
        var factory = new PiecewiseLinearFunctionFactory();
        var f = factory.Bind(parameters) as IDifferentiableFunction;

        Assert.Throws<ArgumentOutOfRangeException>(() => f.Gradient(new Vector(1) { 0 }));
        Assert.Throws<ArgumentOutOfRangeException>(() => f.Gradient(new Vector(1) { 3 }));
    }
    
    [Test]
    public void Gradient_Throws_AtInteriorKnots()
    {
        // (0,0)-(1,2): slope = 2
        // (1,2)-(3,6): slope = 2
        var parameters = new Vector(6) { 0.0, 0.0, 1.0, 2.0, 3.0, 6.0 };
        var factory = new PiecewiseLinearFunctionFactory();
        var f = factory.Bind(parameters) as IDifferentiableFunction;

        // Внутренний узел x=1.0 — градиент не определен
        Assert.Throws<InvalidOperationException>(() => f.Gradient(new Vector(1) { 1 }));
    }
}