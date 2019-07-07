using DepenDarcy;
using DepenDarcy.Core.Entities;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //NugetManager fileReader = new NugetManager();
            //fileReader.FindNugets(@"C:\Source\DepenDarcy\DepenDarcy.Core");

            Project p = new Project();
            p.Analyze(@"C:\Source\DepenDarcy\ConsoleApp1\ConsoleApp1.csproj");

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
