using SuperDuperMenuLib;

class Program
{
    private static int[,] graph_test = {
                {0, 5, Graphs.INF, 10},
                {Graphs.INF, 0, 3, Graphs.INF},
                {Graphs.INF, Graphs.INF, 0, 1},
                {Graphs.INF, Graphs.INF, Graphs.INF, 0}
            };
    static void Main(string[] args)
    {
        SuperDuperMenu menu = new SuperDuperMenu();

        menu.AddEntry("Test of Floyd with one thread on small graph", () => {
            Graphs.FloydOneThread(graph_test, 1, 2, true);
        });
        menu.AddEntry("Test of Floyd with multiple threads on small graph", () => {
            Graphs.FloydMultiThread(graph_test, 1, 2, 4, true);
        });
        menu.AddEntry("Test of Floyd with one thread on big graph", () => {
            Console.Write("Enter number of vertices: ");
            int n = int.Parse(Console.ReadLine());
            int[,] graph = Graphs.GenerateGraph(n);
            Graphs.FloydOneThread(graph, 1, 2, true);
        });
        menu.AddEntry("Test of Floyd with multiple threads on big graph", () => {
            Console.Write("Enter number of vertices: ");
            int n = int.Parse(Console.ReadLine());
            int[,] graph = Graphs.GenerateGraph(n);
            Console.Write("Enter number of threads: ");
            int threads = int.Parse(Console.ReadLine());
            Graphs.FloydMultiThread(graph, 1, 2, threads, true);
        });
        menu.AddEntry("Compare Floyd with one thread and multiple threads", () => {
            Console.Write("Enter number of vertices: ");
            int n = int.Parse(Console.ReadLine());
            int[,] graph = Graphs.GenerateGraph(n);
            Console.Write("Enter number of threads: ");
            int threads = int.Parse(Console.ReadLine());
            long time1 = Graphs.FloydOneThread(graph, 1, 2);
            long time2 = Graphs.FloydMultiThread(graph, 1, 2, threads);
            Console.WriteLine("Time elapsed with one thread: " + time1 + " ms");
            Console.WriteLine("Time elapsed with multiple threads: " + time2 + " ms");
            Console.WriteLine("Time difference: " + Math.Round((float)time1 / time2, 2));
        });
        menu.AddEntry("Best efficiency", () => {
            Console.Write("Enter number of vertices: ");
            int n = int.Parse(Console.ReadLine());
            int[,] graph = Graphs.GenerateGraph(n);
            long time1 = Graphs.FloydOneThread(graph, 1, 2);
            Console.WriteLine("Time elapsed with one thread: " + time1 + " ms");
            long minTime = long.MaxValue;
            int bestThreads = 0;
            for (int i = 2; i <= 16; i++)
            {
                long time2 = Graphs.FloydMultiThread(graph, 1, 2, i);
                if (time2 < minTime)
                    minTime = time2;
                    bestThreads = i;
                Console.WriteLine("Time elapsed with " + i + " threads: " + time2 + " ms");
                Console.WriteLine("Time difference: " + Math.Round((float)time1 / time2, 2));
            }
            Console.WriteLine("Best number of threads: " + bestThreads);
            Console.WriteLine("Best efficiency: " + Math.Round((float)time1 / minTime, 2));
        });




        menu.Title = "Floyd Algorithm";
        menu.Run();
    }
}