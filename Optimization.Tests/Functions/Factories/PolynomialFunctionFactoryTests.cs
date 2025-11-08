using Optimization.Functions;
using Optimization.Functions.Factories;
using Optimization.Functions.Implementations;

namespace Optimization.Tests.Functions.Factories;

public class PolynomialFunctionFactoryTests
{
    [Test]
    public void Function_ValidParameters()
    {
        var f = new PolynomialFunctionFactory();
        var parameters = new Vector(2) { 3.0, 5.0 };

        var function = f.Bind(parameters);

        Assert.That(function, Is.Not.Null);
        Assert.That(function, Is.InstanceOf<PolynomialFunction>());
        Assert.That(function, Is.InstanceOf<IFunction>());

        var value = function.Value(new Vector(1) { 2.0 }); // 3*2 + 5 = 11
        Assert.That(value, Is.EqualTo(11.0).Within(1e-12));
    }
}