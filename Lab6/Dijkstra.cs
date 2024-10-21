using System.Collections.Concurrent;
using System.Diagnostics;

namespace Lab6
{
    static class Dijkstra
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

        public static Dictionary<int, int> DijkstraAlgorithm(Dictionary<int, Dictionary<int, int>> graph, int startVertex, out long elapsedMilliseconds)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            var shortestDistances = graph.Keys.ToDictionary(vertex => vertex, vertex => int.MaxValue);
            var priorityQueue = new SortedSet<(int distance, int vertex)> { (0, startVertex) };
            shortestDistances[startVertex] = 0;

            while (priorityQueue.Any())
            {
                var (currentDistance, currentVertex) = priorityQueue.Min;
                priorityQueue.Remove((currentDistance, currentVertex));

                if (currentDistance > shortestDistances[currentVertex]) continue;

                foreach (var (neighborVertex, edgeWeight) in graph[currentVertex])
                {
                    int distanceThroughCurrent = shortestDistances[currentVertex] + edgeWeight;
                    if (distanceThroughCurrent < shortestDistances[neighborVertex])
                    {
                        priorityQueue.Remove((shortestDistances[neighborVertex], neighborVertex));
                        shortestDistances[neighborVertex] = distanceThroughCurrent;
                        priorityQueue.Add((distanceThroughCurrent, neighborVertex));
                    }
                }
            }

            stopwatch.Stop();
            elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            return shortestDistances;
        }

        public static Dictionary<int, int> DijkstraAlgorithmMultiThreaded(Dictionary<int, Dictionary<int, int>> graph, int startVertex, int threadCount, out long elapsedMilliseconds)
        {
            var stopwatch = Stopwatch.StartNew();
            var shortestDistances = graph.Keys.ToDictionary(v => v, v => v == startVertex ? 0 : int.MaxValue);
            var priorityQueue = new SortedSet<(int distance, int vertex)> { (0, startVertex) };
            var lockObject = new object();

            while (priorityQueue.Any())
            {
                var currentBatch = priorityQueue.Take(threadCount).ToList();
                priorityQueue.RemoveWhere(item => currentBatch.Contains(item));

                var threads = new Thread[currentBatch.Count];
                var countdown = new CountdownEvent(currentBatch.Count);

                for (int i = 0; i < currentBatch.Count; i++)
                {
                    var currentPair = currentBatch[i];
                    threads[i] = new Thread(() =>
                    {
                        ProcessVertex(currentPair, graph, shortestDistances, priorityQueue, lockObject);
                        countdown.Signal();
                    });
                    threads[i].Start();
                }

                countdown.Wait();
            }

            stopwatch.Stop();
            elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            return shortestDistances;
        }

        private static void ProcessVertex(
            (int distance, int vertex) currentPair,
            Dictionary<int, Dictionary<int, int>> graph,
            Dictionary<int, int> shortestDistances,
            SortedSet<(int distance, int vertex)> priorityQueue,
            object lockObject)
        {
            var (currentDistance, currentVertex) = currentPair;
            if (currentDistance > shortestDistances[currentVertex]) return;

            foreach (var (neighborVertex, edgeWeight) in graph[currentVertex])
            {
                int newDistance = currentDistance + edgeWeight;
                if (newDistance < shortestDistances[neighborVertex])
                {
                    lock (lockObject) // lock only when necessary
                    {
                        if (newDistance < shortestDistances[neighborVertex])
                        {
                            shortestDistances[neighborVertex] = newDistance;
                            priorityQueue.RemoveWhere(item => item.vertex == neighborVertex);
                            priorityQueue.Add((newDistance, neighborVertex));
                        }
                    }
                }
            }
        }
    }
}