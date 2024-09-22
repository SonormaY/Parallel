using System.Diagnostics;
static class OneThread
{
    public static void MultiplySmallMatrices(int n, int m, int p)
    {
        // create two small matrices
        int[,] A = new int[n, m];
        int[,] B = new int[m, p];

        // fill them with random numbers
        Misc.FillMatrix(ref A);
        Misc.FillMatrix(ref B);

        // print them out
        Console.WriteLine("Matrix A:");
        Misc.PrintMatrix(A);
        Console.WriteLine("Matrix B:");
        Misc.PrintMatrix(B);

        // multiply them together
        int[,] C = MultiplyMatrices(A, B);

        // print them out
        Console.WriteLine("Matrix C:");
        Misc.PrintMatrix(C);
    }

    public static long MultiplyLargeMatrices(int n, int m, int p, bool print = true)
    {
        // initialize stopwatch
        Stopwatch sw = new Stopwatch();

        // create two large matrices
        int[,] A = new int[n, m];
        int[,] B = new int[m, p];

        // fill them with random numbers
        Misc.FillMatrix(ref A);
        Misc.FillMatrix(ref B);

        // start stopwatch
        sw.Start();

        // multiply them together
        MultiplyMatrices(A, B);

        // stop stopwatch
        sw.Stop();

        // print out time
        if (print) Console.WriteLine("Time elapsed with one thread: " + sw.ElapsedMilliseconds + "ms");
        return sw.ElapsedMilliseconds;
    }
    
    public static long MultiplyLargeMatricesGiven(int[,] A, int[,] B, bool print = true)
    {
        // initialize stopwatch
        Stopwatch sw = new Stopwatch();

        // start stopwatch
        sw.Start();

        // multiply them together
        MultiplyMatrices(A, B);

        // stop stopwatch
        sw.Stop();

        // print out time
        if (print) Console.WriteLine("Time elapsed with one thread: " + sw.ElapsedMilliseconds + "ms");
        return sw.ElapsedMilliseconds;
    }
    static int[,] MultiplyMatrices(int[,] A, int[,] B)
    {
        int n = A.GetLength(0);
        int m = A.GetLength(1);
        int p = B.GetLength(1);
        int[,] C = new int[n, p];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < p; j++)
            {
                C[i, j] = 0;
                for (int k = 0; k < m; k++)
                {
                    C[i, j] += A[i, k] * B[k, j];
                }
            }
        }
        return C;
    }
}