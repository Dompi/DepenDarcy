using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace DepenDarcy.Core.Entities
{
    public class Solution
    {
        public string Name { get; private set; }
        public string Destination { get; private set; }
        public List<Project> Projects { get; set; }

        public Solution()
        {
            this.Projects = new List<Project>();
        }

        public Solution(string destination)
        {
            this.Destination = destination;
            this.Projects = new List<Project>();
        }

        public void Analyze()
        {
            try
            {
                // root
                var root = Path.GetDirectoryName(this.Destination);

                // Get projects
                var lines = File.ReadAllLines(this.Destination).Where(x => x.Contains("Project("));
                foreach (var line in lines)
                {
                    var proj = line.Substring(line.IndexOf('=') + 1).Split(',');
                    this.Projects.Add(
                        new Project(proj[0].Replace("\"", "").Trim(), Path.Combine(root, proj[1].Replace("\"", "").Trim())));
                }

                // Get project dependencies
                foreach (var proj in this.Projects)
                {
                    proj.GetDependencies(this.Projects);
                }

                // Get published nugets
                // From nuspec
                this.GetnugetsNuspec(root);

                // From csproj
                this.GetnugetsCsproj(root);

                // Get nuget dependencies

            }
            catch (System.Exception)
            {
                //TODO
            }

        }

        private void GetnugetsCsproj(string rout)
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
        private void GetnugetsNuspec(string root)
        {
            XmlDocument doc = new XmlDocument();
            foreach (var currentFile in Directory.GetFiles(root, "*.nuspec", SearchOption.AllDirectories))
            {
                try
                {
                    doc.Load(currentFile);
                    var name = doc.GetElementsByTagName("id").Item(0).InnerText;
                    var version = doc.GetElementsByTagName("version").Item(0).InnerText;

                    if (string.IsNullOrEmpty(name) == false)
                    {
                        new Nuget
                        {
                            Name = name,
                            Version = version
                        };
                    }
                }
                catch (System.Exception)
                {
                    //TODO
                }
            }
        }


    }
}
