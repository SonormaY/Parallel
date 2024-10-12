using System.Diagnostics;

public class Graphs
{
	public static int INF = int.MaxValue;
    public static int[,] GenerateGraph(int n)
    {
        Random random = new Random();
        int[,] graph = new int[n, n];
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
            {
                graph[i, j] = random.Next(0, 2) == 0 ? INF : random.Next(1, 10);
                if (i == j)
                    graph[i, j] = 0;
            }
        return graph;
    }

    public static long FloydOneThread(int[,] graph, int a, int b, bool debug = false)
    {
        Stopwatch sw = new Stopwatch();
        
        int n = graph.GetLength(0);
        int[,] d = new int[n, n];
        Array.Copy(graph, d, n * n);

        sw.Start();

        for (int k = 0; k < n; k++) // перебираємо проміжні вершини
            for (int i = 0; i < n; i++) // перебираємо початкові вершини
                for (int j = 0; j < n; j++) // перебираємо кінцеві вершини
                    if (d[i, k] < INF && d[k, j] < INF)
                        d[i, j] = Math.Min(d[i, j], d[i, k] + d[k, j]);

        sw.Stop();
        if (debug)
        {
            Console.WriteLine("Time: " + sw.ElapsedMilliseconds + " ms");
            Console.WriteLine("Distance from " + a + " to " + b + " is " + d[a, b]);
        }
        return sw.ElapsedMilliseconds;
    }

    public static long FloydMultiThread(int[,] graph, int a, int b, int k_threads, bool debug = false)
    {
        Stopwatch sw = new Stopwatch();
        
        int n = graph.GetLength(0);
        int[,] d = new int[n, n];
        Array.Copy(graph, d, n * n);

        sw.Start();

        for (int k = 0; k < n; k++) // перебираємо проміжні вершини
            Parallel.For(0, n, new ParallelOptions { MaxDegreeOfParallelism = k_threads }, i => // перебираємо початкові вершини
            {
                for (int j = 0; j < n; j++) // перебираємо кінцеві вершини
                    if (d[i, k] < INF && d[k, j] < INF)
                        d[i, j] = Math.Min(d[i, j], d[i, k] + d[k, j]);
            }
            );
        
        sw.Stop();
        if (debug)
        {
            Console.WriteLine("Time: " + sw.ElapsedMilliseconds + " ms");
            Console.WriteLine("Distance from " + a + " to " + b + " is " + d[a, b]);
        }
        return sw.ElapsedMilliseconds;
    }
}