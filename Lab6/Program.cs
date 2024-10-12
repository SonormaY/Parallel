using SuperDuperMenuLib;

namespace Lab6
{
    class Program
    {
        static void Main(string[] args)
        {
            SuperDuperMenu menu = new SuperDuperMenu();

            Dictionary<string, Action> menuItems = new Dictionary<string, Action>
            {
                { "One thread on small", () => {}},
                { "One thread on large", () => {}},
                { "Multi threads on small", () => {}},
                { "Multi threads on large", () => {}},
                { "Difference", () => {}},
                { "Best efficiency", () => {}}
            }; 

            menu.Title = "Dijkstra's Shortest Path Algorithm";
            menu.Run();
        }
    }
}