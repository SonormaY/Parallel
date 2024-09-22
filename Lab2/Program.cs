using SuperDuperMenuLib;

var menu = new SuperDuperMenu(new Dictionary<string, Action>
{
    { "1. Multiply two random matrices with one thread", () => {
        Console.WriteLine("Enter the size of the matrix:");
        Console.Write("n: ");
        int n = int.Parse(Console.ReadLine());
        Console.Write("m: ");
        int m = int.Parse(Console.ReadLine());
        Console.Write("p: ");
        int p = int.Parse(Console.ReadLine());

        OneThread.MultiplySmallMatrices(n, m, p);
        
    } },
    { "2. Multiply two large random matrices with one thread", () => {
        Console.WriteLine("Enter the size of the matrix:");
        Console.Write("n: ");
        int n = int.Parse(Console.ReadLine());
        Console.Write("m: ");
        int m = int.Parse(Console.ReadLine());
        Console.Write("p: ");
        int p = int.Parse(Console.ReadLine());

        OneThread.MultiplyLargeMatrices(n, m, p);
    } },
    { "3. Multiply two random matrices with multiple threads", () => {
        Console.WriteLine("Enter the size of the matrix:");
        Console.Write("n: ");
        int n = int.Parse(Console.ReadLine());
        Console.Write("m: ");
        int m = int.Parse(Console.ReadLine());
        Console.Write("p: ");
        int p = int.Parse(Console.ReadLine());
        Console.Write("Threads k: ");
        int k = int.Parse(Console.ReadLine());

        MultiThread.MultiplySmallMatrices(n, m, p, k);
    } },
    { "4. Multiply two large random matrices with multiple threads", () => {
        Console.WriteLine("Enter the size of the matrix:");
        Console.Write("n: ");
        int n = int.Parse(Console.ReadLine());
        Console.Write("m: ");
        int m = int.Parse(Console.ReadLine());
        Console.Write("p: ");
        int p = int.Parse(Console.ReadLine());
        Console.Write("Threads k: ");
        int k = int.Parse(Console.ReadLine());

        MultiThread.MultiplyLargeMatrices(n, m, p, k);
    } },
    { "5. Calculate difference", () => {
        Console.WriteLine("Enter the size of the matrix:");
        Console.Write("n: ");
        int n = int.Parse(Console.ReadLine());
        Console.Write("m: ");
        int m = int.Parse(Console.ReadLine());
        Console.Write("p: ");
        int p = int.Parse(Console.ReadLine());
        Console.Write("Threads k: ");
        int k = int.Parse(Console.ReadLine());

        int[,] A = new int[n, m];
        int[,] B = new int[m, p];
        Misc.FillMatrix(ref A);
        Misc.FillMatrix(ref B);

        float difference = (float)OneThread.MultiplyLargeMatricesGiven(A, B) / MultiThread.MultiplyLargeMatricesGiven(A, B, k);
        Console.WriteLine($"Difference: {Math.Round(difference, 2)}");
    } },
    { "6. Best efficiency", () => {
        Console.WriteLine("Enter the size of the matrices:");
        Console.Write("n: ");
        int n = int.Parse(Console.ReadLine());
        Console.Write("m: ");
        int m = int.Parse(Console.ReadLine());
        Console.Write("p: ");
        int p = int.Parse(Console.ReadLine());

        int[,] A = new int[n, m];
        int[,] B = new int[m, p];
        Misc.FillMatrix(ref A);
        Misc.FillMatrix(ref B);

        int bestK = 0;
        float bestEfficiency = 0;
        float oneThreadTime = (float)OneThread.MultiplyLargeMatricesGiven(A, B, false);
        for (int k = 2; k <= 16; k++)
        {
            Console.WriteLine($"Calculating efficiency with {k} threads...");
            float efficiency = oneThreadTime / MultiThread.MultiplyLargeMatricesGiven(A, B, k, false);
            Console.WriteLine($"Efficiency: {Math.Round(efficiency, 2)}");
            if (efficiency > bestEfficiency)
            {
                bestEfficiency = efficiency;
                bestK = k;
            }
        }
        Console.WriteLine($"Best efficiency: {Math.Round(bestEfficiency, 2)} with {bestK} threads");
    } }
});

menu.Title = "Matrix Operations";
menu.Run();