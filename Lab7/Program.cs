﻿using SuperDuperMenuLib;

namespace Lab7
{
    static class Program
    {
        static void Main(string[] args)
        {
            SuperDuperMenu menu = new SuperDuperMenu();
            var graph = Prim.RandomGraph(20000);
            Dictionary<string, Action> menuItems = new Dictionary<string, Action>
            {
                { "One thread on small", () => {
                        var graph = new Dictionary<int, Dictionary<int, int>>
                    {
                        {0, new Dictionary<int, int> {{1, 4}, {2, 1}}},
                        {1, new Dictionary<int, int> {{3, 1}}},
                        {2, new Dictionary<int, int> {{1, 2}, {3, 5}}},
                        {3, new Dictionary<int, int> {{4, 3}}},
                        {4, new Dictionary<int, int>()}
                    };

                    var (parent, key) = Prim.PrimAlgorithm(graph, 0, out long elapsedMilliseconds);
                    Console.WriteLine($"Elapsed time: {elapsedMilliseconds} ms");
                    Prim.PrintMST(parent, key);
                }},
                { "One thread on large", () => {
                    Prim.PrimAlgorithm(graph, 0, out long elapsedMilliseconds);
                    Console.WriteLine($"Elapsed time: {elapsedMilliseconds} ms");
                }},
                { "Multi threads on small", () => {
                        var graph = new Dictionary<int, Dictionary<int, int>>
                    {
                        {0, new Dictionary<int, int> {{1, 4}, {2, 1}}},
                        {1, new Dictionary<int, int> {{3, 1}}},
                        {2, new Dictionary<int, int> {{1, 2}, {3, 5}}},
                        {3, new Dictionary<int, int> {{4, 3}}},
                        {4, new Dictionary<int, int>()}
                    };

                    Console.WriteLine("Enter the number of threads: ");
                    int k = int.Parse(Console.ReadLine());
                    var (parent, key) = Prim.PrimAlgorithmMultiThreaded(graph, 0, k, out long elapsedMilliseconds);
                    Console.WriteLine($"Elapsed time: {elapsedMilliseconds} ms");
                    Prim.PrintMST(parent, key);
                }},
                { "Multi threads on large", () => {
                    Console.WriteLine("Enter the number of threads: ");
                    int k = int.Parse(Console.ReadLine());
                    Prim.PrimAlgorithmMultiThreaded(graph, 0, k, out long elapsedMilliseconds);
                    Console.WriteLine($"Elapsed time: {elapsedMilliseconds} ms");
                }},
                { "Difference", () => {
                    Console.WriteLine("Enter the number of threads: ");
                    int k = int.Parse(Console.ReadLine());
                    Prim.PrimAlgorithm(graph, 0, out long elapsedMillisecondsSingle);
                    Prim.PrimAlgorithmMultiThreaded(graph, 0, k, out long elapsedMillisecondsMulti);
                    Console.WriteLine($"Single-threaded: {elapsedMillisecondsSingle} ms");
                    Console.WriteLine($"Multi-threaded: {elapsedMillisecondsMulti} ms");
                    Console.WriteLine($"Difference: {(float)elapsedMillisecondsSingle / elapsedMillisecondsMulti}");
                }},
                { "Best efficiency", () => {
                    int bestK = 0;
                    float bestEfficiency = 0;
                    Prim.PrimAlgorithm(graph, 0, out long elapsedMillisecondsSingle);
                    for (int k = 1; k <= 16; k++)
                    {
                        Prim.PrimAlgorithmMultiThreaded(graph, 0, k, out long elapsedMillisecondsMulti);
                        float efficiency = (float)elapsedMillisecondsSingle / elapsedMillisecondsMulti;
                        Console.WriteLine($"Threads: {k}, Efficiency: {efficiency}");
                        if (efficiency > bestEfficiency)
                        {
                            bestEfficiency = efficiency;
                            bestK = k;
                        }
                    }
                    Console.WriteLine($"Best efficiency: {bestEfficiency}");
                    Console.WriteLine($"Best number of threads: {bestK}");
                }}
            }; 
            menu.LoadEntries(menuItems);

            menu.Title = "Prim's MST Algorithm";
            menu.Run();
        }
    }
}