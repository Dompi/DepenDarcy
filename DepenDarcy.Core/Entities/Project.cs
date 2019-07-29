﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using DepenDarcy.Core.Extensions;

namespace DepenDarcy.Core.Entities
{
    public class Project
    {
        private readonly ILogger logger;

        public string Name { get; set; }
        public string Destination { get; set; }
        public List<Project> Dependencies { get; set; }
        public List<Project> Dependents { get; set; }
        public List<Nuget> PublishedNugets { get; set; }
        public List<Nuget> UsedNugets { get; set; }

        public Project(ILogger logger)
        {
            this.logger = logger;
            this.Dependencies = new List<Project>();
            this.Dependents = new List<Project>();
            this.PublishedNugets = new List<Nuget>();
            this.UsedNugets = new List<Nuget>();
        }
        public Project(string destination, ILogger logger)
        {
            this.logger = logger;
            this.Destination = destination;
            this.Name = Path.GetFileName(destination);
            this.Dependencies = new List<Project>();
            this.Dependents = new List<Project>();
            this.PublishedNugets = new List<Nuget>();
            this.UsedNugets = new List<Nuget>();
        }
        public Project(string name, string destination, ILogger logger)
        {
            this.logger = logger;
            this.Name = name;
            this.Destination = destination;
            this.Dependencies = new List<Project>();
            this.Dependents = new List<Project>();
            this.PublishedNugets = new List<Nuget>();
            this.UsedNugets = new List<Nuget>();
        }

        public void GetProjectDependencies(List<Project> projects)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(this.Destination);
                if (doc.TryGetElementsByTagName("ProjectReference", out XmlNodeList xmlNodeList))
                {
                    for (int i = 0; i < xmlNodeList.Count; i++)
                    {
                        var projNameBeta = xmlNodeList.Item(i).OuterXml.Split('\\').Single(x => x.Contains(".csproj"));
                        var projName = projNameBeta.Substring(0, projNameBeta.IndexOf(".csproj"));
                        var proj = projects.SingleOrDefault(x => x.Name.Equals(projName));
                        if (proj != null)
                        {
                            this.Dependencies.Add(proj);
                            proj.Dependents.Add(this);
                        }
                        else
                        {
                            //TODO
                            this.logger.LogDebug($"There are no Project: {projName} in Destination: {this.Destination}");
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                //TODO
                this.logger.LogDebug($"There are some issue reading Project in Destination: {this.Destination}");

            }
        }
        public void GetPublishedNugets()
        {
            this.GetPublishedNugetsCsproj();
            this.GetPublishedNugetsNuspec();
        }
        public void GetUsedNugets()
        {
            this.GetUsedNugetsCsproj();
            this.GetUsedNugetsPackages();
        }

        private void GetPublishedNugetsCsproj()
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(this.Destination);
                if (doc.TryGetElementsByTagName("GeneratePackageOnBuild", out XmlNodeList xmlNodeList))
                {
                    if (bool.Parse(xmlNodeList.Item(0).InnerText))
                    {
                        string name, version;
                        if (doc.TryGetElementsByTagName("id", out XmlNodeList xmlNodeId))
                        {
                            name = xmlNodeId.Item(0).InnerText;
                        }
                        else
                        {
                            name = this.Name;
                        }
                        if (doc.TryGetElementsByTagName("version", out XmlNodeList xmlNodeVersion))
                        {
                            version = xmlNodeVersion.Item(0).InnerText;
                        }
                        else
                        {
                            version = "1.0.0";
                        }

                        if (string.IsNullOrEmpty(name) == false)
                        {
                            this.PublishedNugets.Add(
                                new Nuget
                                {
                                    Name = name,
                                    Version = version
                                });
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                //TODO
                this.logger.LogDebug($"There are some issue reading document in Destination: {this.Destination}");
            }
        }
        private void GetPublishedNugetsNuspec()
        {
            XmlDocument doc = new XmlDocument();
            var root = Path.GetDirectoryName(this.Destination);
            foreach (var currentFile in Directory.GetFiles(root, "*.nuspec", SearchOption.AllDirectories))
            {
                if (currentFile.Contains("bin") || currentFile.Contains("obj"))
                {
                    continue;
                }
                try
                {
                    string version;
                    doc.Load(currentFile);
                    var name = doc.GetElementsByTagName("id").Item(0).InnerText;
                    if (doc.TryGetElementsByTagName("version", out XmlNodeList xmlNodeVersion))
                    {
                        version = xmlNodeVersion.Item(0).InnerText;
                    }
                    else
                    {
                        // TODO from assembly
                        version = "1.0.0";
                    }

                    if (string.IsNullOrEmpty(name) == false)
                    {
                        this.PublishedNugets.Add(
                        new Nuget
                        {
                            Name = name,
                            Version = version
                        });
                    }
                }
                catch (Exception e)
                {
                    this.logger.LogDebug($"There are some issue reading document in Destination: {currentFile} message: {e.Message}");
                    //TODO
                }
            }
        }

        private void GetUsedNugetsCsproj()
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(this.Destination);
                if (doc.TryGetElementsByTagName("PackageReference", out XmlNodeList xmlNodePackageReference))
                {
                    for (int i = 0; i < xmlNodePackageReference.Count; i++)
                    {
                        string version = null;
                        try
                        {
                            version = xmlNodePackageReference[i].Attributes["Version"].Value;
                        }
                        catch (Exception e)
                        {
                            for (int j = 0; j < xmlNodePackageReference[i].ChildNodes.Count; j++)
                            {
                                version = xmlNodePackageReference[i].ChildNodes[j].InnerText;
                            }
                        }

                        this.UsedNugets.Add(new Nuget
                        {
                            Name = xmlNodePackageReference[i].Attributes["Include"].Value,
                            Version = version
                        });
                    }
                }

            }
            catch (Exception e)
            {
                //TODO
                this.logger.LogDebug($"There are some issue reading document in Destination: {this.Destination}");
            }
        }
        private void GetUsedNugetsPackages()
        {
            var root = Path.GetDirectoryName(this.Destination);
            foreach (var currentFile in Directory.GetFiles(root, "packages.config", SearchOption.AllDirectories))
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(currentFile);
                    if (doc.TryGetElementsByTagName("packages", out XmlNodeList xmlNodePackages))
                    {
                        for (int i = 0; i < xmlNodePackages[0].ChildNodes.Count; i++)
                        {
                            this.UsedNugets.Add(new Nuget
                            {
                                Name = xmlNodePackages[0].ChildNodes[i].Attributes["id"].Value,
                                Version = xmlNodePackages[0].ChildNodes[i].Attributes["version"].Value
                            });
                        }
                    }

                }
                catch (Exception e)
                {
                    //TODO
                    this.logger.LogDebug($"There are some issue reading document in Destination: {currentFile} message {e.Message}");
                }
            }
        }
    }
}
