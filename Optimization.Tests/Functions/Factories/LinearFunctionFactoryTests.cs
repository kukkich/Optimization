using Optimization.Functions;
using Optimization.Functions.Factories;
using Optimization.Functions.Implementations;

namespace Optimization.Tests.Functions.Factories;

[TestFixture]
public class LinearFunctionFactoryTests
{
    [Test]
    public void Function_ValidParameters()
    {
        var f = new LinearFunctionFactory();
        var parameters = new Vector(2) { 3.0, 5.0 }; // w=3, b=5

        var function = f.Bind(parameters);

        Assert.That(function, Is.Not.Null);
        Assert.That(function, Is.InstanceOf<LinearFunction>());
        Assert.That(function, Is.InstanceOf<IDifferentiableFunction>());

        var value = function.Value(new Vector(1) { 2.0 }); // 3*2 + 5 = 11
        Assert.That(value, Is.EqualTo(11.0).Within(1e-12));
    }
}