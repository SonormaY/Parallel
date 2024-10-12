namespace Lab6
{
    class Dijkstra
    {
        static Dictionary<int, Dictionary<int, int>> RandomGraph(int n)
        {
            var rng = new Random();
            var result = new Dictionary<int, Dictionary<int, int>>();

            for (int i = 0; i < n; i++)
            {
                result[i] = Enumerable.Range(0, n)
                    .Where(j => j != i)
                    .OrderBy(_ => rng.Next())
                    .Take(rng.Next(1, 11))
                    .ToDictionary(j => j, _ => rng.Next(1, 101));
            }

            return result;
        }

        
    }
}