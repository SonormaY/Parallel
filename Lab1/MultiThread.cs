using System.Diagnostics;
static class MultiThread
{
    public static void AddSmallMatrices(int n, int m, int k)
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
        int[,] C = AddMatrices(A, B, k);

        // print them out
        Console.WriteLine("Matrix C:");
        Misc.PrintMatrix(C);
    }

    public static long AddLargeMatrices(int n, int m, int k, bool print = true)
    {
        // initialize stopwatch
        Stopwatch sw = new Stopwatch();

        // create two large matrices
        int[,] A = new int[n, m];
        int[,] B = new int[n, m];

        // fill them with random numbers
        Misc.FillMatrix(ref A);
        Misc.FillMatrix(ref B);

        // start stopwatch
        sw.Start();

        // add them together
        AddMatrices(A, B, k);

        // stop stopwatch
        sw.Stop();

        // print out time
        if (print) Console.WriteLine("Time elapsed with multithread: " + sw.ElapsedMilliseconds + "ms");
        return sw.ElapsedMilliseconds;
    }

    public static long AddLargeMatricesGiven(int[,] A, int[,] B, int k, bool print = true)
    {
        // initialize stopwatch
        Stopwatch sw = new Stopwatch();

        // start stopwatch
        sw.Start();

        // add them together
        AddMatrices(A, B, k);

        // stop stopwatch
        sw.Stop();

        // print out time
        if (print) Console.WriteLine("Time elapsed with multithread: " + sw.ElapsedMilliseconds + "ms");
        return sw.ElapsedMilliseconds;
    }

    public static long SubtractLargeMatrices(int n, int m, int k, bool print = true)
    {
        // initialize stopwatch
        Stopwatch sw = new Stopwatch();

        // create two large matrices
        int[,] A = new int[n, m];
        int[,] B = new int[n, m];

        // fill them with random numbers
        Misc.FillMatrix(ref A);
        Misc.FillMatrix(ref B);

        // start stopwatch
        sw.Start();

        // subtract them
        int[,] C = SubtractMatrices(A, B, k);

        // stop stopwatch
        sw.Stop();

        // print out time
        if (print) Console.WriteLine("Time elapsed with multithread: " + sw.ElapsedMilliseconds + "ms");
        return sw.ElapsedMilliseconds;
    }

    // add two large matrices with threading
    static int[,] AddMatrices(int[,] A, int[,] B, int k)
    {
        int n = A.GetLength(0);
        int m = A.GetLength(1);
        int[,] C = new int[n, m];
        
        // create threads
        Thread[] threads = new Thread[k];

        for (int i = 0; i < k; i++)
        {
            threads[i] = new Thread((object obj) =>
            {
                for (int j = (int)obj; j < m; j += k)
                {
                    for (int l = 0; l < n; l++)
                    {
                        C[l, j] = A[l, j] + B[l, j];
                    }
                }
            });
            threads[i].Start(i);
        }

        // wait for all threads to finish
        for (int i = 0; i < k; i++)
        {
            threads[i].Join();
        }        

        return C;
    }

    // subtract two large matrices with threading
    static int[,] SubtractMatrices(int[,] A, int[,] B, int k)
    {
        int n = A.GetLength(0);
        int m = A.GetLength(1);
        int[,] C = new int[n, m];

        // create threads
        Thread[] threads = new Thread[n];

        for (int i = 0; i < k; i++)
        {
            threads[i] = new Thread((object obj) =>
            {
                for (int j = (int)obj; j < m; j += k)
                {
                    for (int l = 0; l < n; l++)
                    {
                        C[l, j] = A[l, j] - B[l, j];
                    }
                }
            });
            threads[i].Start(i);
        }

        // wait for all threads to finish
        for (int i = 0; i < k; i++)
        {
            threads[i].Join();
        }

        return C;
    }
}