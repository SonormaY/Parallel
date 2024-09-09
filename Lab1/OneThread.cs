using System.Diagnostics;
static class OneThread
{
    // add two small matrices to see it work
    public static void AddSmallMatrices(int n, int m)
    {
        // create two small matrices
        int[,] A = new int[n, m];
        int[,] B = new int[n, m];

        // fill them with random numbers
        Misc.FillMatrix(ref A);
        Misc.FillMatrix(ref B);

        // print them out
        Console.WriteLine("Matrix A:");
        Misc.PrintMatrix(A);
        Console.WriteLine("Matrix B:");
        Misc.PrintMatrix(B);


        // add them together
        int[,] C = AddMatrices(A, B);

        // print them out
        Console.WriteLine("Matrix C:");
        Misc.PrintMatrix(C);
    }

    // add two large matrices to stopwatch it
    public static long AddLargeMatrices(int n, int m, bool print = true)
    {
        // initialize stopwatch
        Stopwatch sw = new Stopwatch();

        // create two large matrices
        int[,] A = new int[n, m];
        int[,] B = new int[n, m];

        // fill them with random numbers
        Misc.FillMatrix(ref A);
        Misc.FillMatrix(ref B);

        // start stopwatch and add them together 
        sw.Start();
        AddMatrices(A, B);
        sw.Stop();
        if (print) Console.WriteLine("Time elapsed with one thread: " + sw.ElapsedMilliseconds + "ms");
        return sw.ElapsedMilliseconds;
    }

    public static long AddLargeMatricesGiven(int[,] A, int[,] B, bool print = true)
    {
        // initialize stopwatch
        Stopwatch sw = new Stopwatch();

        // start stopwatch and add them together 
        sw.Start();
        AddMatrices(A, B);
        sw.Stop();
        if (print) Console.WriteLine("Time elapsed with one thread: " + sw.ElapsedMilliseconds + "ms");
        return sw.ElapsedMilliseconds;
    }

    // subtract two large matrices to stopwatch it
    public static long SubtractLargeMatrices(int n, int m, bool print = true)
    {
        // initialize stopwatch
        Stopwatch sw = new Stopwatch();

        // create two large matrices
        int[,] A = new int[n, m];
        int[,] B = new int[n, m];

        // fill them with random numbers
        Misc.FillMatrix(ref A);
        Misc.FillMatrix(ref B);

        // start stopwatch and subtract them
        sw.Start();
        SubstractMatrices(A, B);
        sw.Stop();
        if (print) Console.WriteLine("Time elapsed with one thread: " + sw.ElapsedMilliseconds + "ms");
        return sw.ElapsedMilliseconds;
    }

    static int[,] AddMatrices(int[,] A, int[,] B)
    {
        int n = A.GetLength(0);
        int m = A.GetLength(1);
        int[,] C = new int[n, m];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                C[i, j] = A[i, j] + B[i, j];
            }
        }
        return C;

    }
    static int[,] SubstractMatrices(int[,] A, int[,] B)
    {
        int n = A.GetLength(0);
        int m = A.GetLength(1);
        int[,] C = new int[n, m];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                C[i, j] = A[i, j] - B[i, j];
            }
        }
        return C;
    }
}