class Misc
{
    static Random random = new Random();

    public static (double[][] A, double[] b) GenerateConvergentSystem(int size, double diagonalDominanceFactor = 1.5)
    {
        double[][] A = new double[size][];
        double[] b = new double[size];

        for (int i = 0; i < size; i++)
        {
            A[i] = new double[size];
            double rowSum = 0;

            // Fill non-diagonal elements
            for (int j = 0; j < size; j++)
            {
                if (i != j)
                {
                    A[i][j] = random.NextDouble() * 2 - 1; // Random value between -1 and 1
                    rowSum += Math.Abs(A[i][j]);
                }
            }

            // Make the matrix diagonally dominant
            A[i][i] = rowSum * diagonalDominanceFactor * (random.Next(2) == 0 ? 1 : -1);

            // Generate b
            b[i] = random.NextDouble() * 100; // Random value between 0 and 100
        }

        return (A, b);
    }

    public static void PrintMatrix(double[][] A)
    {
        for (int i = 0; i < A.Length; i++)
        {
            for (int j = 0; j < A[i].Length; j++)
            {
                Console.Write($"{A[i][j]:F2}\t");
            }
            Console.WriteLine();
        }
    }

    public static void PrintVector(double[] v)
    {
        foreach (var value in v)
        {
            Console.Write($"{value:F2}\t");
        }
        Console.WriteLine();
    }
}