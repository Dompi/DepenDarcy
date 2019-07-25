using DepenDarcy.Core.Entities;
using System.IO;
using System.Xml;
using DepenDarcy.Core.Extensions;

namespace ConsoleApp1
{
    class Program
    {
        private static string root = @"C:\Sources\TECUTEST";
        //private static string root = @"C:\Source\DepenDarcy";
        private static string currentFile = @"C:\Hasznos\Temp\Teemp.nuspec";
        static void Main(string[] args)
        {
            Graph graph = new Graph( new MyLogger());
            graph.BuildGraph(root);
        }
    }
}
