using System.Diagnostics;
static class OneThread
{
    public static void SmallSystem()
    {
        double[][] A =
        {
            [4, 2, 2],
            [2, 3, 2],
            [-1, 1, 1]
        };
        double[] B = {8, 7, 1};
        Console.WriteLine("A:");
        Misc.PrintMatrix(A);
        Console.WriteLine("B:");
        Misc.PrintVector(B);

        double eps = 0.001;
        double[] result = Solve(A, B, eps);

        if (result != null)
        {
            Console.WriteLine($"Solution: [{string.Join(", ", result)}]");
        }
    }

    public static float BigSystem(int n, double [][] A = null, double[] b = null)
    {
        //initialize stopwatch
        Stopwatch sw = new Stopwatch();

        if (A == null || b == null)
        {
            (A, b) = Misc.GenerateConvergentSystem(n);
        }

        double eps = 0.001;
        sw.Start();
        Solve(A, b, eps);
        sw.Stop();
        Console.WriteLine($"Elapsed time for {n}x{n} matrix: {sw.ElapsedMilliseconds} ms"); 
        return sw.ElapsedMilliseconds;       
    }

    public static double[] Solve(double[][] A, double[] B, double eps, int max_iterations = 100, bool debug = false)
    {
        int length = B.Length;
        double[] preparedB = new double[length];
        for (int i = 0; i < length; i++)
        {
            preparedB[i] = B[i] / A[i][i];
        }

        double[][] preparedA = A.Select(row => row.ToArray()).ToArray();
        // matrix preparation
        for (int i = 0; i < length; i++)
        {
            double temp = preparedA[i][i];
            for (int j = 0; j < length; j++)
            {
                preparedA[i][j] = -(preparedA[i][j] / temp);
            }
            preparedA[i][i] = 0;
        }

        double[] x = (double[])preparedB.Clone();
        double[] x_prev = (double[])preparedB.Clone();

        for (int k = 0; k < max_iterations; k++)
        {
            double max_error = 0;
            for (int i = 0; i < length; i++)
            {
                double temp = 0;
                for (int j = 0; j < length; j++)
                {
                    temp += preparedA[i][j] * x_prev[j];
                }
                x[i] = Math.Round(preparedB[i] + temp, 4);
                double error = Math.Abs(x[i] - x_prev[i]);
                max_error = Math.Max(max_error, error);
            }
            Array.Copy(x, x_prev, length);
            if (debug) Console.WriteLine(max_error);
            if (max_error < eps)
            {
                return x;
            }
            if (debug) Console.WriteLine($"Iteration ({k + 1}): [{string.Join(", ", x)}]");
        }
        Console.WriteLine("The system did not converge");
        return [];
    }   
}