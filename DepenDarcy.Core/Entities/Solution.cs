﻿using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace DepenDarcy.Core.Entities
{
    public class Solution
    {
        private readonly ILogger logger;


        public string Name { get; private set; }
        public string Destination { get; private set; }
        public List<Project> Projects { get; set; }

        public Solution(ILogger logger)
        {
            this.logger = logger;
            this.Projects = new List<Project>();
        }
        public Solution(string destination, ILogger logger)
        {
            this.logger = logger;
            this.Name = Path.GetFileName(destination).Replace(".sln", "").Trim();
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
                        new Project(proj[0].Replace("\"", "").Trim(), Path.Combine(root, proj[1].Replace("\"", "").Trim()), this.logger));
                }

                foreach (var proj in this.Projects)
                {
                    // Get project dependencies
                    proj.GetProjectDependencies(this.Projects);

                    // Get published nugets
                    proj.GetPublishedNugets();

                    // Get used nugets 
                    proj.GetUsedNugets();
                }

                // Get project dependendents
                // Get nuget dependencies

            }
            catch (System.Exception)
            {
                //TODO
            }

        }
    }
}
