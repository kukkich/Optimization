using Optimization.Functions.Factories;
using Optimization.Functions.Implementations;

namespace Optimization.Tests.Functions.Implementations;

public class PolynomialFunctionTests
{
    [Test]
    public void Value_Returns_Constant_When_SingleCoefficient()
    {
        // params: [5] => 5;
        var parameters = new Vector(1) {5.0};
        var factory = new PolynomialFunctionFactory();
        var f = factory.Bind(parameters);

        Assert.That(f.Value(new Vector(1) {0.0}), Is.EqualTo(5.0));
        Assert.That(f.Value(new Vector(1) {123.456}), Is.EqualTo(5.0));
        Assert.That(f.Value(new Vector(1) {-10.0}), Is.EqualTo(5.0));
    }
    
    [Test]
    public void Value_At_Zero_Returns_LastCoefficient()
    {
        // params: [7, 8, 9] => 7x^2 + 8x + 9; f(0) = 9
        var parameters = new Vector(3) {7.0, 8.0, 9.0};
        var factory = new PolynomialFunctionFactory();
        var f = factory.Bind(parameters);

        Assert.That(f.Value(new Vector(1) {0.0}), Is.EqualTo(9.0).Within(1e-12));
    }
    
    [Test]
    public void Value_Computes_Polynomial_Correctly_NegativeX()
    {
        // params: [-1, 2, -3, 4] => -1*x^3 + 2*x^2 - 3*x + 4
        var parameters = new Vector(4) {-1.0, 2.0, -3.0, 4.0};
        var factory = new PolynomialFunctionFactory();
        var f = factory.Bind(parameters);

        Assert.That(f.Value(new Vector(1) {-2.0}), Is.EqualTo(26).Within(1e-12));
    }
}