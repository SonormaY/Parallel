static class Misc
{
    public static void FillMatrix(ref int[,] A)
    {
        int n = A.GetLength(0);
        int m = A.GetLength(1);
        Random rand = new Random();
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                A[i, j] = rand.Next(10);
            }
        }
    }
    public static void PrintMatrix(int[,] A)
    {
        int n = A.GetLength(0);
        int m = A.GetLength(1);
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                Console.Write(A[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}