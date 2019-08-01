using DepenDarcy.Core.Entities;
using System;
using System.Collections.Generic;
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


            var a = graph.GetBoundydContexts();



            System.Console.WriteLine("---------------------------------------");
            foreach (var item in graph.GetNugetDependenciesDFS(graph.Projects.Single(x => x.Name.Equals("ResponsibleGaming"))))
            {
                System.Console.WriteLine($"Level: {item.Key}");
                foreach (var proj in item.Value)
                {
                    System.Console.WriteLine($"Project to refresh: {proj.Name}");
                }
            }


            //System.Console.WriteLine("---------------------------------------");
            //foreach (var item in graph.GetNugetProject())
            //{
            //    System.Console.WriteLine($"Nuget: {item.Key}");
            //    foreach (var proj in item.Value)
            //    {
            //        System.Console.WriteLine($"Project {proj.ProjectName } version: {proj.NugetVersion}");
            //    }

            //}

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"UsedNugetLog.txt"))
            {
                foreach (var item in graph.GetNugetProject())
                {
                    file.WriteLine($"Nuget: {item.Key}");
                    foreach (var proj in item.Value)
                    {
                        file.WriteLine($"   Project {proj.ProjectName } version: {proj.NugetVersion}");
                    }

                }
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"NugetLog.txt"))
            {
                foreach (var item in graph.GetNugetProject())
                {
                    file.WriteLine($"{item.Key}, ");
                }
            }



            Console.ReadLine();
        }
    }
}
