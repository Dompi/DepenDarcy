﻿using DepenDarcy.Core.Entities;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        private static string root = @"C:\Sources\TECUTEST\accountverification";
        //private static string root = @"C:\Sources\TECUTEST";
        //private static string root = @"C:\Source\DepenDarcy";
        static void Main(string[] args)
        {

            //var aa = Directory.GetFiles(@"C:\Source", "packages.config", SearchOption.AllDirectories);


            Graph graph = new Graph(root, new MyLogger());
            graph.BuildGraph();

            var a = 5;

            //NugetManager fileReader = new NugetManager();
            //fileReader.FindNugets(@"C:\Source\DepenDarcy\DepenDarcy.Core");

            //Project p = new Project(@"C:\Source\DepenDarcy\ConsoleApp1\ConsoleApp1.csproj");
            //p.Analyze();

            //List<Solution> solutions = new List<Solution>();

            //foreach (var currentFile in Directory.GetFiles(root, "*.sln", SearchOption.AllDirectories))
            //{
            //    Solution s = new Solution(currentFile, new MyLogger());
            //    s.Analyze();
            //    solutions.Add(s);
            //}


            //var a = solutions.Count;

            //string root = @"C:\Sources\TECUTEST";

            //fileReader.AddNuget(new Nuget { Name = "Microsoft.Extensions.Logging.Abstractions", Version = "1.1.1" });
            //fileReader.FindPublishedNugets(root);
            //var a  = fileReader.GetDependencies(root);


            //Console.WriteLine("---------------------------------------------------------------");
            //foreach (var item in a)
            //{
            //    Console.WriteLine($"NugetName: {item.NugetName}, NugetVersion: {item.NugetVersion}, ProjectName: {item.ProjectName}");
            //}
            //Console.WriteLine("---------------------------------------------------------------");

            //Console.ReadLine();
        }
    }
}
