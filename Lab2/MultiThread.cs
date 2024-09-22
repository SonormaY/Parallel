using System.Diagnostics;
static class MultiThread
{
    public static void MultiplySmallMatrices(int n, int m, int p, int k)
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
        int[,] C = MultiplyMatrices(A, B, k);

        // print them out
        Console.WriteLine("Matrix C:");
        Misc.PrintMatrix(C);
    }

    public static long MultiplyLargeMatrices(int n, int m, int p, int k, bool print = true)
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
        MultiplyMatrices(A, B, k);

        // stop stopwatch
        sw.Stop();

        // print out time
        if (print) Console.WriteLine("Time elapsed with " + k + " threads: " + sw.ElapsedMilliseconds + "ms");
        return sw.ElapsedMilliseconds;
    }

    public static long MultiplyLargeMatricesGiven(int[,] A, int[,] B, int k, bool print = true)
    {
        // initialize stopwatch
        Stopwatch sw = new Stopwatch();

        // start stopwatch
        sw.Start();

        // multiply them together
        MultiplyMatrices(A, B, k);

        // stop stopwatch
        sw.Stop();

        // print out time
        if (print) Console.WriteLine("Time elapsed with " + k + " threads: " + sw.ElapsedMilliseconds + "ms");
        return sw.ElapsedMilliseconds;
    }
    static int[,] MultiplyMatrices(int[,] A, int[,] B, int k)
    {
        int n = A.GetLength(0);
        int m = A.GetLength(1);
        int p = B.GetLength(1);
        int[,] C = new int[n, p];
        

        // create threads
        Thread[] threads = new Thread[k];
        
        int completedThreads = 0;
        for (int i = 0; i < k; i++)
        {
            threads[i] = new Thread((object obj) =>
            {
                for (int j = (int)obj; j < p; j += k)
                {
                    for (int l = 0; l < n; l++)
                    {
                        C[l, j] = 0;
                        for (int o = 0; o < m; o++)
                        {
                            C[l, j] += A[l, o] * B[o, j];
                        }
                    }
                }
                Interlocked.Increment(ref completedThreads);
            });
            threads[i].Start(i);
        }
        // wait for all threads to finish
        while (completedThreads < k) { 
            Thread.Sleep(1);
        }
        
        return C;
    }
}