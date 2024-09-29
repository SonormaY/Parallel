using System.Diagnostics;
static class MultiThread
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
    public static float BigSystem(int n,  int k, double [][] A = null, double[] b = null)
    {
        //initialize stopwatch
        Stopwatch sw = new Stopwatch();

        if (A == null || b == null)
        {
            (A, b) = Misc.GenerateConvergentSystem(n);
        }

        double eps = 0.001;
        sw.Start();
        Solve(A, b, eps, k: k);
        sw.Stop();
        Console.WriteLine($"Elapsed time for {n}x{n} matrix: {sw.ElapsedMilliseconds} ms"); 
        return sw.ElapsedMilliseconds;       
    }
    public static double[] Solve(double[][] A, double[] B, double eps, int max_iterations = 100, int k = 2, bool debug = false)
    {
        // create threads
        Thread[] threads = new Thread[k];
        
        int completedThreads = 0;

        // matrix preparation
        int length = B.Length;
        double[] preparedB = new double[length];
        for (int i = 0; i < k; i++)
        {
            threads[i] = new Thread((object obj) =>
            {
                for (int j = (int)obj; j < length; j += k)
                {
                    preparedB[j] = B[j] / A[j][j];
                }
                Interlocked.Increment(ref completedThreads);
            });
            threads[i].Start(i);
        }
        // wait for all threads to finish
        while (completedThreads < k) { 
            Thread.Sleep(1);
        }

        double[][] preparedA = A.Select(row => row.ToArray()).ToArray();
        // matrix preparation
        for (int i = 0; i < k; i++)
        {
            threads[i] = new Thread((object obj) =>
            {
                for (int j = (int)obj; j < length; j += k)
                {
                    double temp = preparedA[j][j];
                    for (int l = 0; l < length; l++)
                    {
                        preparedA[j][l] = -(preparedA[j][l] / temp);
                    }
                    preparedA[j][j] = 0;
                }
                Interlocked.Increment(ref completedThreads);
            });
            threads[i].Start(i);
        }

        // wait for all threads to finish
        while (completedThreads < k) { 
            Thread.Sleep(1);
        }

        double[] x = (double[])preparedB.Clone();
        double[] x_prev = (double[])preparedB.Clone();

        for (int l = 0; l < max_iterations; l++)
        {
            double max_error = 0;
            for (int i = 0; i < k; i++)
            {
                threads[i] = new Thread((object obj) =>
                {
                    for (int j = (int)obj; j < length; j += k)
                    {
                        double temp = 0;
                        for (int l = 0; l < length; l++)
                        {
                            temp += preparedA[j][l] * x_prev[l];
                        }
                        x[j] = Math.Round(preparedB[j] + temp, 4);
                        double error = Math.Abs(x[j] - x_prev[j]);
                        max_error = Math.Max(max_error, error);
                    }
                    Interlocked.Increment(ref completedThreads);
                });
                threads[i].Start(i);
            }
            // wait for all threads to finish
            while (completedThreads < k) { 
                Thread.Sleep(1);
            }
            Array.Copy(x, x_prev, length);
            if (debug) Console.WriteLine(max_error);
            if (max_error < eps)
            {
                return x;
            }
            if (debug)Console.WriteLine($"Iteration ({l + 1}): [{string.Join(", ", x)}]");
        }
        if (debug) Console.WriteLine("The system did not converge");
        return [];
    }
}