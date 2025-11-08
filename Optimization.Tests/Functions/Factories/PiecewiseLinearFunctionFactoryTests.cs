using Optimization.Functions;
using Optimization.Functions.Factories;
using Optimization.Functions.Implementations;

namespace Optimization.Tests.Functions.Factories;

public class PiecewiseLinearFunctionFactoryTests
{
    [Test]
    public void Function_ValidParameters()
    {
        var f = new PiecewiseLinearFunctionFactory();
        var parameters = new Vector(4) { 0.0, 0.0, 1.0, 1.0 };
        var function = f.Bind(parameters);

        Assert.That(function, Is.Not.Null);
        Assert.That(function, Is.InstanceOf<PiecewiseLinearFunction>());
        Assert.That(function, Is.InstanceOf<IDifferentiableFunction>());

        var value = function.Value(new Vector(1) { 0.5 });
        Assert.That(value, Is.EqualTo(0.5).Within(1e-12));
    }
    
    [Test]
    public void Factory_Bind_Throws_When_OddParameterCount()
    {
        var factory = new PiecewiseLinearFunctionFactory();
        Assert.Throws<ArgumentException>(() => factory.Bind(new Vector(5) {0.0, 0.0, 1.0, 2.0, 3.0}));
    }

    [Test]
    public void Factory_Bind_Throws_When_Abscissas_NotStrictlyIncreasing_Equal()
    {
        var factory = new PiecewiseLinearFunctionFactory();
        Assert.Throws<ArgumentException>(() => factory.Bind(new Vector(4) {0.0, 0.0, 0.0, 1.0}));
    }

    [Test]
    public void Factory_Bind_Throws_When_Abscissas_NotStrictlyIncreasing_Decreasing()
    {
        var factory = new PiecewiseLinearFunctionFactory();
        Assert.Throws<ArgumentException>(() => factory.Bind(new Vector(4) {1.0, 0.0, 0.0, 1.0}));
    }
}