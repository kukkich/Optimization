using Optimization.Extensions;
using Optimization.Functions.Utils;

namespace Optimization.Functions.Implementations;

public class NaturalCubicSpline : IFunction
{
    private readonly IVector _knots;
    private readonly Vector _constCoefficients;
    private readonly Vector _linearCoefficients;
    private readonly Vector _quadraticCoefficients;
    private readonly Vector _cubicCoefficients;
    private readonly int _knotCount;

    public NaturalCubicSpline(IVector knots, IVector values)
    {
        _knotCount = knots.Count;
        _knots = knots;

        _constCoefficients = new Vector(_knotCount - 1);
        _linearCoefficients = new Vector(_knotCount - 1);
        _quadraticCoefficients = new Vector(_knotCount - 1);
        _cubicCoefficients = new Vector(_knotCount - 1);

        BuildCoefficients(knots, values);
    }

    public double Value(IVector point)
    {
        if (point.Count != 1)
        {
            throw new ArgumentException("Point must be one-dimensional", nameof(point));
        }

        var xValue = point[0];
        if (xValue < _knots[0] || xValue > _knots[_knotCount - 1])
        {
            throw new ArgumentOutOfRangeException(nameof(point), "Point is out of domain of definition [x0, xN]");
        }

        var intervalIndex = SegmentIndexFinder.Find(_knots, xValue);
        var localX = xValue - _knots[intervalIndex];

        return _constCoefficients[intervalIndex]
               + _linearCoefficients[intervalIndex] * localX
               + _quadraticCoefficients[intervalIndex] * localX * localX
               + _cubicCoefficients[intervalIndex] * localX * localX * localX;
    }

    private void BuildCoefficients(IVector knots, IVector values)
    {
        var segmentCount = _knotCount - 1;

        var segmentLengths = new Vector(segmentCount);
        for (var i = 0; i < segmentCount; i++)
        {
            var segmentLength = knots[i + 1] - knots[i];
            segmentLengths.Add(segmentLength);
        }

        var secondDerivativesAtKnots = new Vector(_knotCount);
        for (var i = 0; i < _knotCount; i++)
        {
            secondDerivativesAtKnots.Add(0.0);
        }

        var interiorKnotCount = _knotCount - 2;
        if (interiorKnotCount > 0)
        {
            var lowerDiag = new Vector(interiorKnotCount);
            var mainDiag = new Vector(interiorKnotCount);
            var upperDiag = new Vector(interiorKnotCount);
            var rhs = new Vector(interiorKnotCount);

            for (var i = 0; i < interiorKnotCount; i++)
            {
                var knotIndex = i + 1;
                var hLeft = segmentLengths[knotIndex - 1];
                var hRight = segmentLengths[knotIndex];

                lowerDiag.Add(hLeft);
                mainDiag.Add(2.0 * (hLeft + hRight));
                upperDiag.Add(hRight);

                var slopeRight = (values[knotIndex + 1] - values[knotIndex]) / hRight;
                var slopeLeft = (values[knotIndex] - values[knotIndex - 1]) / hLeft;
                rhs.Add(6.0 * (slopeRight - slopeLeft));
            }

            var secondDerivativesInterior = new Vector(interiorKnotCount);
            SolveTridiagonal(lowerDiag, mainDiag, upperDiag, rhs, secondDerivativesInterior);

            for (var i = 0; i < interiorKnotCount; i++)
            {
                var knotIndex = i + 1;
                secondDerivativesAtKnots[knotIndex] = secondDerivativesInterior[i];
            }
        }
        
        for (var i = 0; i < segmentCount; i++)
        {
            var h = segmentLengths[i];
            var bi = (values[i + 1] - values[i]) / h - h * (2.0 * secondDerivativesAtKnots[i] + secondDerivativesAtKnots[i + 1]) / 6.0;

            _constCoefficients.Add(values[i]);
            _linearCoefficients.Add(bi);
            _quadraticCoefficients.Add(secondDerivativesAtKnots[i] / 2.0);
            _cubicCoefficients.Add((secondDerivativesAtKnots[i + 1] - secondDerivativesAtKnots[i]) / (6.0 * h));
        }
    }

    private static void SolveTridiagonal(IVector lowerDiag, IVector mainDiag, IVector upperDiag, IVector rhs, IVector solutionOut)
    {
        var size = rhs.Count;
        if (lowerDiag.Count != size || mainDiag.Count != size || upperDiag.Count != size)
        {
            throw new ArgumentException("Coefficients must have same length");
        }

        if (size == 0)
        {
            return;
        }

        var upperModified = new Vector(size);
        var rhsModified = new Vector(size);
        for (var i = 0; i < size; i++)
        {
            upperModified.Add(0.0);
            rhsModified.Add(0.0);
        }

        var pivot = mainDiag[0];
        if (pivot == 0.0)
        {
            throw new InvalidOperationException("b[0] == 0");
        }

        upperModified[0] = (size > 1) ? upperDiag[0] / pivot : 0.0;
        rhsModified[0] = rhs[0] / pivot;

        for (var i = 1; i < size; i++)
        {
            pivot = mainDiag[i] - lowerDiag[i] * upperModified[i - 1];
            if (pivot == 0.0)
            {
                throw new InvalidOperationException("Degenerate tridiagonal system.");
            }

            upperModified[i] = (i < size - 1) ? upperDiag[i] / pivot : 0.0;
            rhsModified[i] = (rhs[i] - lowerDiag[i] * rhsModified[i - 1]) / pivot;
        }

        for (var i = 0; i < size; i++)
        {
            solutionOut.Add(0.0);
        }

        solutionOut[size - 1] = rhsModified[size - 1];
        for (var i = size - 2; i >= 0; i--)
        {
            solutionOut[i] = rhsModified[i] - upperModified[i] * solutionOut[i + 1];
        }
    }
}