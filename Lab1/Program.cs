using SuperDuperMenuLib;

var menu = new SuperDuperMenu(new Dictionary<string, Action>
{
    { "1. Add two random matrix with one thread", () => {
        Console.WriteLine("Enter the size of the matrix:");
        Console.Write("n: ");
        int n = int.Parse(Console.ReadLine());
        Console.Write("m: ");
        int m = int.Parse(Console.ReadLine());

        OneThread.AddSmallMatrices(n, m);
        
    } },
    { "2. Add two large random matrices with one thread", () => {
        Console.WriteLine("Enter the size of the matrix:");
        Console.Write("n: ");
        int n = int.Parse(Console.ReadLine());
        Console.Write("m: ");
        int m = int.Parse(Console.ReadLine());

        OneThread.AddLargeMatrices(n, m);
    } },
    { "3. Subtract two large random matrices with one thread", () => {
        Console.WriteLine("Enter the size of the matrix:");
        Console.Write("n: ");
        int n = int.Parse(Console.ReadLine());
        Console.Write("m: ");
        int m = int.Parse(Console.ReadLine());

        OneThread.SubtractLargeMatrices(n, m);
    } },
    { "4. Add two random matrix with multiple threads", () => {
        Console.WriteLine("Enter the size of the matrix:");
        Console.Write("n: ");
        int n = int.Parse(Console.ReadLine());
        Console.Write("m: ");
        int m = int.Parse(Console.ReadLine());
        Console.Write("Threads k: ");
        int k = int.Parse(Console.ReadLine());

        MultiThread.AddSmallMatrices(n, m, k);
    } },
    { "5. Add two large random matrices with multiple threads", () => {
        Console.WriteLine("Enter the size of the matrix:");
        Console.Write("n: ");
        int n = int.Parse(Console.ReadLine());
        Console.Write("m: ");
        int m = int.Parse(Console.ReadLine());
        Console.Write("Threads k: ");
        int k = int.Parse(Console.ReadLine());

        MultiThread.AddLargeMatrices(n, m, k);
    } },
    { "6. Subtract two large random matrices with multiple threads", () => {
        Console.WriteLine("Enter the size of the matrix:");
        Console.Write("n: ");
        int n = int.Parse(Console.ReadLine());
        Console.Write("m: ");
        int m = int.Parse(Console.ReadLine());
        Console.Write("Threads k: ");
        int k = int.Parse(Console.ReadLine());

        MultiThread.SubtractLargeMatrices(n, m, k);
    } },
    { "7. Calculate difference", () => {
        Console.WriteLine("Enter the size of the matrix:");
        Console.Write("n: ");
        int n = int.Parse(Console.ReadLine());
        Console.Write("m: ");
        int m = int.Parse(Console.ReadLine());
        Console.Write("Threads k: ");
        int k = int.Parse(Console.ReadLine());

        int[,] A = new int[n, m];
        int[,] B = new int[n, m];
        Misc.FillMatrix(ref A);
        Misc.FillMatrix(ref B);

        float difference = (float)OneThread.AddLargeMatricesGiven(A, B) / MultiThread.AddLargeMatricesGiven(A, B, k);
        Console.WriteLine($"Difference: {Math.Round(difference, 2)}");
    } },
    { "8. Best efficiency", () => {
        Console.WriteLine("Enter the size of the matrix:");
        Console.Write("n: ");
        int n = int.Parse(Console.ReadLine());
        Console.Write("m: ");
        int m = int.Parse(Console.ReadLine());

        int[,] A = new int[n, m];
        int[,] B = new int[n, m];
        Misc.FillMatrix(ref A);
        Misc.FillMatrix(ref B);

        int bestK = 0;
        float bestEfficiency = 0;
        for (int k = 2; k <= 16; k++)
        {
            Console.WriteLine($"Calculating efficiency with {k} threads...");
            float efficiency = (float)OneThread.AddLargeMatricesGiven(A, B, false) / MultiThread.AddLargeMatricesGiven(A, B, k, false);
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