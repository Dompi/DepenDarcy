using DepenDarcy;
using DepenDarcy.Core.Entities;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            FileReader fileReader = new FileReader();
            //fileReader.FindNugets(@"C:\Source\DepenDarcy\DepenDarcy.Core");

            string root = @"C:\Source\Sensor";

            fileReader.AddNuget(new Nuget { Name = "Microsoft.Extensions.Logging.Abstractions", Version = "1.1.1" });
            fileReader.FindNugets(root);
            fileReader.GetDependencies(root);

            Console.ReadLine();
        }
    }
}
