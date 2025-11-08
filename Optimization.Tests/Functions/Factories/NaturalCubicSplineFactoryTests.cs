using Optimization.Functions;
using Optimization.Functions.Factories;
using Optimization.Functions.Implementations;

namespace Optimization.Tests.Functions.Factories;

[TestFixture]
public class NaturalCubicSplineFactoryTests
{
    [Test]
    public void Function_ValidParameters()
    {
        var parameters = new Vector(8) { 0.0, 0.0, 1.0, 1.0, 4.0, 5.0, 5.0, 6.0 };
        var factory = new NaturalCubicSplineFactory();
        var function = factory.Bind(parameters);

        Assert.That(function, Is.Not.Null);
        Assert.That(function, Is.InstanceOf<NaturalCubicSpline>());
        Assert.That(function, Is.InstanceOf<IDifferentiableFunction>());
    }
}