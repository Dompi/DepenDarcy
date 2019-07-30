using DepenDarcy.Core.Entities;
using System;
using System.Linq;

namespace ConsoleApp1
{
    internal class Program
    {
        private static string root = @"C:\Sources\TECUTEST";

        //private static string root = @"C:\Source\DepenDarcy";
        //private static string currentFile = @"C:\Hasznos\Temp\Teemp.nuspec";
        private static void Main(string[] args)
        {
            Graph graph = new Graph(new MyLogger());
            graph.BuildGraph(root);

            var a = graph.GetDependencies(graph.Projects.Single(x => x.Name.Equals("Techsson.Platform.Customer.Nucleus")));


            System.Console.WriteLine("---------------------------------------");
            foreach (var item in a)
            {
                System.Console.WriteLine($"Level: {item.Key}");
                foreach (var proj in item.Value)
                {
                    System.Console.WriteLine($"Project to refresh: {proj.Name}");
                }
            }

            Console.ReadLine();
        }
    }
}
