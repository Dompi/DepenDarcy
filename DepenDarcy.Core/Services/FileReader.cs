using DepenDarcy.Core;
using DepenDarcy.Core.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace DepenDarcy
{
    public class FileReader
    {
        private XmlDocument doc;
        private readonly string nugetIdentifier;
        private readonly List<Nuget> nugets;
//        private readonly List<string> exactFiles;
        private readonly List<string> fileTypes;


        public FileReader()
        {
            doc = new XmlDocument();
            nugetIdentifier = "*.nuspec";
            nugets = new List<Nuget>();
//            exactFiles = new List<string> { "packages.config" };
            fileTypes = new List<string> { "*.csproj", "packages.config" };

        }

        public void AddNuget(Nuget nuget)
        {
            if(nugets.Any(x=>x.Name.Equals(nuget.Name)) == false)
            {
                nugets.Add(nuget);
            }
        }
        public void FindNugets(string rout)
        {
            foreach (var currentFile in Directory.GetFiles(rout, nugetIdentifier, SearchOption.AllDirectories))
            {
                doc.Load(currentFile);
                nugets.Add(
                    new Nuget
                    {
                        Name = doc.GetElementsByTagName("id").Item(0).InnerText,
                        Version = doc.GetElementsByTagName("version").Item(0).InnerText
                    });
            }
        }

        public List<Dependency> GetDependencies(string rout)
        {
            List<Dependency> dependencies = new List<Dependency>();
            foreach (var pattern in fileTypes)
            {
                foreach (var currentFile in Directory.GetFiles(rout, pattern, SearchOption.AllDirectories))
                {
                    try
                    {   // Open the text file using a stream reader.
                        using (StreamReader sr = new StreamReader(currentFile))
                        {
                            foreach (var nuget in nugets)
                            {
                                // Read the stream to a string, and write the string to the console.
                                string line = sr.ReadToEnd();
                                if (line.Contains(pattern))
                                {
                                    dependencies.Add(
                                        new Dependency
                                        {
                                            NugetName = nuget.Name,
                                            ProjectName = currentFile,
                                            NugetVersion = nuget.Version
                                        });
                                }
                            }
                        }
                    }
                    catch (IOException e)
                    {
                    }
                }
            }

            return dependencies;
        }
    }
}
