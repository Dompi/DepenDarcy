using DepenDarcy.Core;
using DepenDarcy.Core.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace DepenDarcy
{
    public class NugetManager
    {
        private readonly List<Nuget> publishedNugets;
        private readonly List<NugetLocation> nugetLocations;

        public NugetManager()
        {
            publishedNugets = new List<Nuget>();
            nugetLocations = new List<NugetLocation>
            {
                new NugetLocation { FileType = "*.csproj", NugetLocationType = NugetLocationType.Project },
                new NugetLocation { FileType = "packages.config", NugetLocationType = NugetLocationType.Packages }
            };
        }
        public void AddPublishedNugets(Nuget nuget)
        {
            if (publishedNugets.Any(x => x.Name.Equals(nuget.Name)) == false)
            {
                publishedNugets.Add(nuget);
            }
        }
        public void FindPublishedNugets(string rout)
        {
            List<Nuget> nugets = GetnugetsCsproj(rout);
            nugets .AddRange(GetnugetsNuspec(rout));
        }
        public List<Dependency> GetDependencies(string rout)
        {
            List<Dependency> dependencies = new List<Dependency>();
            System.Console.WriteLine("--------------------------------");
            foreach (var pattern in fileTypes)
            {
                foreach (var currentFile in Directory.GetFiles(rout, pattern, SearchOption.AllDirectories))
                {
                    try
                    {
                        System.Console.WriteLine(currentFile);
                        // Open the text file using a stream reader.
                        using (StreamReader sr = new StreamReader(currentFile))
                        {
                            // Read the stream to a string, and write the string to the console.
                            string line = sr.ReadToEnd();
                            foreach (var nuget in nugets)
                            {
                                if (line.Contains(nuget.Name))
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
            System.Console.WriteLine("--------------------------------");

            return dependencies;
        }


        private List<Nuget> GetnugetsCsproj(string rout)
        {
            List<Nuget> nugets = new List<Nuget>();
            XmlDocument doc = new XmlDocument();
            foreach (var currentFile in Directory.GetFiles(rout, "*.csproj", SearchOption.AllDirectories))
            {
                try
                {
                    doc.Load(currentFile);
                    var name = doc.GetElementsByTagName("id").Item(0).InnerText;
                    var version = doc.GetElementsByTagName("version").Item(0).InnerText;

                    if (string.IsNullOrEmpty(name) == false)
                    {
                        nugets.Add(
                            new Nuget
                            {
                                Name = name,
                                Version = version
                            });
                    }
                }
                catch (System.Exception)
                {
                    //TODO
                }
            }
            return nugets;
        }
        private List<Nuget> GetnugetsNuspec(string rout)
        {
            List<Nuget> nugets = new List<Nuget>();
            XmlDocument doc = new XmlDocument();
            foreach (var currentFile in Directory.GetFiles(rout, "*.nuspec", SearchOption.AllDirectories))
            {
                try
                {
                    doc.Load(currentFile);
                    var name = doc.GetElementsByTagName("id").Item(0).InnerText;
                    var version = doc.GetElementsByTagName("version").Item(0).InnerText;

                    if (string.IsNullOrEmpty(name) == false)
                    {
                        nugets.Add(
                            new Nuget
                            {
                                Name = name,
                                Version = version
                            });
                    }
                }
                catch (System.Exception)
                {
                    //TODO
                }
            }
            return nugets;
        }


    }
}
