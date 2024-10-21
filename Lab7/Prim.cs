using System.Collections.Concurrent;
using System.Diagnostics;

namespace Lab7;

public static class Prim
{
    public static Dictionary<int, Dictionary<int, int>> RandomGraph(int n)
        {
            var result = new ConcurrentDictionary<int, Dictionary<int, int>>();
            var randomPool = new ConcurrentBag<Random>();

            Parallel.For(0, Environment.ProcessorCount, _ =>
            {
                randomPool.Add(new Random(Guid.NewGuid().GetHashCode()));
            });

            Parallel.For(0, n, i =>
            {
                var localRandom = randomPool.TryTake(out var random) ? random : new Random(Guid.NewGuid().GetHashCode());

                var edges = new Dictionary<int, int>();
                var edgeCount = localRandom.Next(1, Math.Min(11, n));
                var possibleNeighbors = Enumerable.Range(0, n).Where(j => j != i).ToList();
                
                for (int k = 0; k < edgeCount && possibleNeighbors.Count > 0; k++)
                {
                    int index = localRandom.Next(possibleNeighbors.Count);
                    int neighbor = possibleNeighbors[index];
                    possibleNeighbors.RemoveAt(index);
                    edges[neighbor] = localRandom.Next(1, 101);
                }

                result[i] = edges;

                if (!randomPool.IsEmpty)
                    randomPool.Add(localRandom);
            });

            return new Dictionary<int, Dictionary<int, int>>(result);
        }

    public static (Dictionary<int, int>, Dictionary<int, int>) PrimAlgorithm(
        Dictionary<int, Dictionary<int, int>> graph, 
        int startVertex, 
        out long elapsedMilliseconds)
    {
        var stopwatch = Stopwatch.StartNew();
        var parent = new Dictionary<int, int>();
        var key = new Dictionary<int, int>();
        var mstSet = new Dictionary<int, bool>();

        foreach (var vertex in graph.Keys)
        {
            key[vertex] = int.MaxValue;
            mstSet[vertex] = false;
        }

        key[startVertex] = 0;
        parent[startVertex] = -1;

        for (int count = 0; count < graph.Count - 1; count++)
        {
            int u = MinKey(key, mstSet);
            if (u == -1 || !graph.ContainsKey(u)) continue;

            mstSet[u] = true;
            foreach (var v in graph[u].Keys)
            {
                if (!mstSet[v] && graph[u][v] < key[v])
                {
                    parent[v] = u;
                    key[v] = graph[u][v];
                }
            }
        }

        stopwatch.Stop();
        elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

        return (parent, key);
    } 

    public static (Dictionary<int, int>, Dictionary<int, int>) PrimAlgorithmMultiThreaded(
    Dictionary<int, Dictionary<int, int>> graph, 
    int startVertex, 
    int k, 
    out long elapsedMilliseconds)
    {
        var stopwatch = Stopwatch.StartNew();
        var parent = new Dictionary<int, int>();
        var key = new Dictionary<int, int>();
        var mstSet = new Dictionary<int, bool>();
        var lockObject = new object();
        
        foreach (var vertex in graph.Keys)
        {
            key[vertex] = int.MaxValue;
            mstSet[vertex] = false;
        }
        key[startVertex] = 0;
        parent[startVertex] = -1;

        var vertices = graph.Keys.Where(v => v != startVertex).ToList();
        var chunkSize = (int)Math.Ceiling(vertices.Count / (double)k);
        var resetEvents = new ManualResetEvent[k];

        for (int i = 0; i < k; i++)
        {
            resetEvents[i] = new ManualResetEvent(false);
            int threadIndex = i;
            
            new Thread(() =>
            {
                int start = threadIndex * chunkSize;
                int end = Math.Min(start + chunkSize, vertices.Count);
                
                for (int count = 0; count < end - start; count++)
                {
                    int u;
                    lock (lockObject)
                    {
                        u = MinKey(key, mstSet);
                        if (u != -1 && graph.ContainsKey(u))
                            mstSet[u] = true;
                    }

                    if (u == -1 || !graph.ContainsKey(u)) continue;

                    foreach (var v in graph[u].Keys)
                    {
                        if (!mstSet[v] && graph[u][v] < key[v])
                        {
                            lock (lockObject)
                            {
                                if (!mstSet[v] && graph[u][v] < key[v])
                                {
                                    parent[v] = u;
                                    key[v] = graph[u][v];
                                }
                            }
                        }
                    }
                }
                
                resetEvents[threadIndex].Set();
            }).Start();
        }

        WaitHandle.WaitAll(resetEvents);
        
        // диспоуз ManualResetEvent
        foreach (var evt in resetEvents)
        {
            evt.Dispose();
        }

        stopwatch.Stop();
        elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
        return (parent, key);
    }

    private static int MinKey(Dictionary<int, int> key, Dictionary<int, bool> mstSet)
    {
        int min = int.MaxValue;
        int minIndex = -1;

        foreach (var vertex in key.Keys)
        {
            if (!mstSet[vertex] && key[vertex] < min)
            {
                min = key[vertex];
                minIndex = vertex;
            }
        }

        return minIndex;
    }

    public static void PrintMST(Dictionary<int, int> parent, Dictionary<int, int> key)
    {
        Console.WriteLine("Edge \tWeight");
        foreach (var vertex in parent.Keys) {
            if (parent[vertex] != -1) {
                Console.WriteLine($"{parent[vertex]} - {vertex} \t{key[vertex]}");
            }
        }
    }
}