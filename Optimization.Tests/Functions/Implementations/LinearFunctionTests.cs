using NUnit.Framework.Legacy;
using Optimization.Functions;
using Optimization.Functions.Factories;
using Optimization.Functions.Implementations;

namespace Optimization.Tests.Functions.Implementations;

[TestFixture]
public class LinearFunctionTests
{
    [Test]
    public void Value_Computes_Correctly_SingleDimension()
    {
        // params: [3, 5] => 3x + 5; dim = 1; f(2) = 11
        var parameters = new Vector(2) { 3.0, 5.0 };
        var factory = new LinearFunctionFactory();
        var f = factory.Bind(parameters);
        
        Assert.That(f.Value(new Vector(1) { 2.0 }), Is.EqualTo(11.0).Within(1e-12));
    }

    [Test]
    public void Value_Computes_Correctly_MultiDimension()
    {
        // params = [1.5, -2, 0, 4] => 1.5x - 2y +0z + 4; dim = 3; f(2, 10, 7) = -13
        var parameters = new Vector(4) { 1.5, -2.0, 0.0, 4.0 };
        var factory = new LinearFunctionFactory();
        var f = factory.Bind(parameters);

        Assert.That(f.Value(new Vector(3) { 2.0, 10.0, 7.0 }), Is.EqualTo(-13.0).Within(1e-12));
    }

    [Test]
    public void Value_Uses_Bias_When_Weights_Are_Zero()
    {
        // params = [0, -7] => 0x - 7; dim = 1; f(3) = -7
        var parameters = new Vector(2) { 0.0, -7.0 };
        var factory = new LinearFunctionFactory();
        var f = factory.Bind(parameters);

        Assert.That(f.Value(new Vector(1) { 3.0 }), Is.EqualTo(-7.0).Within(1e-12));
    }
}