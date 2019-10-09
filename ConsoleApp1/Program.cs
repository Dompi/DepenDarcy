using DepenDarcy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    internal class Program
    {
        private static string root = @"C:\Sources\TECUTEST2";

        //private static string root = @"C:\Source\DepenDarcy";
        //private static string currentFile = @"C:\Hasznos\Temp\Teemp.nuspec";
        private static void Main(string[] args)
        {
            Graph graph = new Graph(new MyLogger());
            graph.BuildGraph(root);

            // Techsson.Platform.Customers.Domain
            // Techsson.Platform.Customer.Application

            System.Console.WriteLine("----------------------**********Techsson.Platform.Customers.Domain***********-----------------");
            foreach (var proj in graph.Projects.Where(x => x.UsedNugets.Select(s => s.Name).Contains("Techsson.Platform.Customers.Domain")))
            {
                System.Console.WriteLine($"Solution Name: {proj.SolutionName}, Project to refresh: {proj.Name}, nuget version: {proj.UsedNugets.Single(x=>x.Name == "Techsson.Platform.Customers.Domain").Version}");
            }
            System.Console.WriteLine("----------------------*********************-----------------");





            System.Console.WriteLine("----------------------**********Techsson.Platform.Customer.Application***********-----------------");
            foreach (var proj in graph.Projects.Where(x => x.UsedNugets.Select(s => s.Name).Contains("Techsson.Platform.Customer.Application")))
            {
                System.Console.WriteLine($"Solution Name: {proj.SolutionName}, Project to refresh: {proj.Name} nuget version: {proj.UsedNugets.Single(x => x.Name == "Techsson.Platform.Customer.Application").Version}");
            }
            System.Console.WriteLine("----------------------*********************-----------------");

            var a = graph.GetBoundydContexts();



            System.Console.WriteLine("---------------------------------------");
            foreach (var item in graph.GetNugetDependenciesDFS(graph.Projects.Single(x => x.Name.Equals("Techsson.Platform.Customers.Domain"))))
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
