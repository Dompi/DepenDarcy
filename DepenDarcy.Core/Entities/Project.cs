using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace DepenDarcy.Core.Entities
{
    public class Project
    {
        public string Name { get; set; }
        public string Destination { get; set; }
        public List<Project> Dependencies { get; set; }
        public List<Project> Dependents { get; set; }
        public List<Nuget> PublishedNugets { get; set; }
        public List<Nuget> UsedNugets { get; set; }

        public Project()
        {
            this.Dependencies = new List<Project>();
            this.Dependents = new List<Project>();
            this.PublishedNugets = new List<Nuget>();
            this.UsedNugets = new List<Nuget>();
        }
        public Project(string destination)
        {
            this.Destination = destination;
            this.Dependencies = new List<Project>();
            this.Dependents = new List<Project>();
            this.PublishedNugets = new List<Nuget>();
            this.UsedNugets = new List<Nuget>();
        }
        public Project(string name, string destination)
        {
            this.Name = name;
            this.Destination = destination;
            this.Dependencies = new List<Project>();
            this.Dependents = new List<Project>();
            this.PublishedNugets = new List<Nuget>();
            this.UsedNugets = new List<Nuget>();
        }

        public void GetDependencies(List<Project> projects)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(this.Destination);

                // Get dependencies
                try
                {
                    for (int i = 0; i < doc.GetElementsByTagName("ProjectReference").Count; i++)
                    {
                        var projNameBeta =
                                doc.GetElementsByTagName("ProjectReference").Item(0).OuterXml
                                    .Split('\\')
                                    .Single(x => x.Contains(".csproj"));
                        var projName = projNameBeta.Substring(0, projNameBeta.IndexOf(".csproj"));
                        var proj = projects.SingleOrDefault(x => x.Name.Equals(projName));
                        if (proj != null)
                        {
                            this.Dependencies.Add(proj);
                        }
                    }
                }
                catch (System.Exception)
                {
                    //TODO
                }

            }
            catch (System.Exception)
            {
                //TODO
            }
        }

        public void Analyze()
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(this.Destination);

                // Get dependencies
                try
                {
                    for (int i = 0; i < doc.GetElementsByTagName("ProjectReference").Count; i++)
                    {
                        var projName = doc.GetElementsByTagName("ProjectReference").Item(0).OuterXml.Split('\\').Single(x => x.Contains(".csproj")).Trim('"', ' ', '/', '>');
                        this.Dependencies.Add(new Project(projName));
                    }
                }
                catch (System.Exception)
                {
                    //TODO
                }

                // Get nuget
                try
                {
                    if (bool.Parse(doc.GetElementsByTagName("GeneratePackageOnBuild").Item(0).InnerText) == true)
                    {
                        var name = doc.GetElementsByTagName("PackageId").Item(0).InnerText;
                        var version = doc.GetElementsByTagName("Version").Item(0).InnerText;

                        List<Dependency> dependencies = new List<Dependency>();
                        for (int i = 0; i < doc.GetElementsByTagName("dependency").Count; i++)
                        {

                        }

                        if (string.IsNullOrEmpty(name) == false)
                        {
                            this.PublishedNugets.Add(
                                new Nuget
                                {
                                    Name = name,
                                    Version = version,
                                    Dependencies = dependencies
                                });
                        }

                    }
                }
                catch (System.Exception)
                {
                    //TODO
                }
            }
            catch (System.Exception)
            {
                //TODO
            }
        }
    }
}
