using Optimization.Functions.Factories;

namespace Optimization.Tests.Functions.Implementations;

[TestFixture]
public class NaturalCubicSplineTests
{
    [Test]
    public void LinearData_IsInterpolatedExactly()
    {
        // y = 2x + 1 на узлах [-2, -1, 0, 1, 3]
        var parameters = new Vector(10);
        parameters.Add(-2); parameters.Add(2 * -2 + 1); // -3
        parameters.Add(-1); parameters.Add(2 * -1 + 1); // -1
        parameters.Add(0); parameters.Add(2 * 0 + 1); // 1
        parameters.Add(1); parameters.Add(2 * 1 + 1); // 3
        parameters.Add(3); parameters.Add(2 * 3 + 1); // 7
        var pf = new NaturalCubicSplineFactory();
        var f = pf.Bind(parameters);

        // Проверка значений в узлах
        Assert.That(f.Value(Vec( -2)), Is.EqualTo(-3).Within(1e-12));
        Assert.That(f.Value(Vec( -1)), Is.EqualTo(-1).Within(1e-12));
        Assert.That(f.Value(Vec(  0)), Is.EqualTo( 1).Within(1e-12));
        Assert.That(f.Value(Vec(  1)), Is.EqualTo( 3).Within(1e-12));
        Assert.That(f.Value(Vec(  3)), Is.EqualTo( 7).Within(1e-12));

        // Проверка нескольких внутренних точек — должно быть ровно 2x+1
        Assert.That(f.Value(Vec(-1.5)), Is.EqualTo(2 * -1.5 + 1).Within(1e-12));
        Assert.That(f.Value(Vec( 0.5)), Is.EqualTo(2 *  0.5 + 1).Within(1e-12));
        Assert.That(f.Value(Vec( 2.0)), Is.EqualTo(2 *  2.0 + 1).Within(1e-12));
    }
    
    private static IVector Vec(double x)
    {
        var v = new Vector(1);
        v.Add(x);
        return v;
    }
}