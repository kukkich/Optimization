namespace Optimization.Optimizators.EquationSystem;

public class GaussElimination : ISLAESolver
{
    private readonly double _precision;

    public GaussElimination(double precision = 1e-3)
    {
        _precision = precision;
    }

    public IVector Solve(IMatrix leftPart, IVector rightPart)
    {
        try
        {
            ForwardElimination(leftPart, rightPart);
            return BackSubstitution(leftPart, rightPart);
        }
        catch (Exception)
        {
            throw new DivideByZeroException();
        }
    }

    private void ForwardElimination(IMatrix leftPart, IVector rightPart)
    {
        for (var i = 0; i < leftPart.Count - 1; i++)
        {
            var max = Math.Abs(leftPart[i][i]);

            var rowNumber = i;

            for (var j = i + 1; j < leftPart.Count; j++)
            {
                if (max < Math.Abs(leftPart[j][i]))
                {
                    max = Math.Abs(leftPart[j][i]);
                    rowNumber = j;
                }
            }
            if (rowNumber != i)
            {
                for (var j = 0; j < leftPart[i].Count; j++)
                {
                    (leftPart[rowNumber][j], leftPart[i][j]) =
                        (leftPart[i][j], leftPart[rowNumber][j]);
                }

                (rightPart[i], rightPart[rowNumber]) =
                    (rightPart[rowNumber], rightPart[i]);
            }

            if (Math.Abs(leftPart[i][i]) > _precision)
            {
                for (var j = i + 1; j < leftPart.Count; j++)
                {
                    var coefficient = leftPart[j][i] / leftPart[i][i];
                    leftPart[j][i] = 0d;
                    rightPart[j] -= coefficient * rightPart[i];

                    for (var k = i + 1; k < leftPart.Count; k++)
                    {
                        leftPart[j][k] -= coefficient * leftPart[i][k];
                    }
                }
            }
            else throw new DivideByZeroException();
        }
    }

    private Vector BackSubstitution(IMatrix leftPart, IVector rightPart)
    {
        var solution = new Vector(rightPart.Count);

        for (var i = leftPart.Count - 1; i >= 0; i--)
        {
            var sum = 0d;

            for (var j = i + 1; j < leftPart.Count; j++)
            {
                sum += leftPart[i][j] * solution[j];
            }

            if (Math.Abs(leftPart[i][i]) > _precision)
                solution[i] = (rightPart[i] - sum) / leftPart[i][i];
            else throw new DivideByZeroException();
        }

        return solution;
    }
}