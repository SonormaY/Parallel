using SuperDuperMenuLib;

var menu = new SuperDuperMenu(new Dictionary<string, Action>
{
    { "1. Solve small system with one thread", () => {
        OneThread.SmallSystem();
    }},
    { "2. Solve big system with one thread", () => {
        Console.WriteLine("Enter the size of the system:");
        int n = int.Parse(Console.ReadLine());
        OneThread.BigSystem(n);
    }},
    { "3. Solve small system with multiple threads", () => {
        MultiThread.SmallSystem();
    }},
    { "4. Solve big system with multiple threads", () => {
        Console.WriteLine("Enter the size of the system:");
        int n = int.Parse(Console.ReadLine());
        Console.WriteLine("Enter the number of threads:");
        int k = int.Parse(Console.ReadLine());
        MultiThread.BigSystem(n, k);
    }},
    { "5. Calculate difference", () => {
        Console.WriteLine("Enter the size of the system:");
        Console.Write("n: ");
        int n = int.Parse(Console.ReadLine());
        Console.Write("Threads k: ");
        int k = int.Parse(Console.ReadLine());
        double[][] A;
        double[] B;
        (A, B) = Misc.GenerateConvergentSystem(n);

        float difference = OneThread.BigSystem(n, A, B) / MultiThread.BigSystem(n, k, A, B);
        Console.WriteLine($"Difference: {Math.Round(difference, 2)}");
    } },
    { "6. Best efficiency", () => {
        Console.WriteLine("Enter the size of the system:");
        Console.Write("n: ");
        int n = int.Parse(Console.ReadLine());

        double[][] A;
        double[] B;
        (A, B) = Misc.GenerateConvergentSystem(n);
        int bestK = 0;
        float bestEfficiency = 0;
        float oneThreadTime = (float)OneThread.BigSystem(n, A, B);
        for (int k = 2; k <= 16; k++)
        {
            Console.WriteLine($"Calculating efficiency with {k} threads...");
            float efficiency = oneThreadTime / MultiThread.BigSystem(n, k, A, B);
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

menu.Title = "Jacoby method";
menu.Run();