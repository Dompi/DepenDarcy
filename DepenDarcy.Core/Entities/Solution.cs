using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            this.Analyze();
        }
        public Solution(string destination, ILogger logger)
        {
            this.logger = logger;
            this.Name = Path.GetFileName(destination).Replace(".sln", "").Trim();
            this.Destination = destination;
            this.Projects = new List<Project>();
            this.Analyze();
        }

        private void Analyze()
        {
            try
            {
                // root
                var root = Path.GetDirectoryName(this.Destination);

                // Get projects
                var lines = File.ReadAllLines(this.Destination).Where(x => x.Contains("Project("));
                foreach (var line in lines)
                {
                    try
                    {
                        var proj = line.Substring(line.IndexOf('=') + 1).Split(',');
                        if (proj[1].Contains(".csproj") || proj[1].Contains(".sqlproj"))
                        {
                            this.Projects.Add(
                                new Project(
                                        proj[0].Replace("\"", "").Trim(),
                                        Path.Combine(root, proj[1].Replace("\"", "").Trim()), this.logger)
                                );
                        }
                        else
                        {
                            // TODO
                        }

                    }
                    catch (Exception e)
                    {
                        this.logger.LogDebug($"There are some issue reading sln in Destination: {this.Destination} exp message: {e.Message}");
                    }
                }

                // Get dependencies and nugets
                foreach (var proj in this.Projects)
                {
                    // Get project dependencies
                    try
                    {
                        proj.GetProjectDependencies(this.Projects);
                    }
                    catch (Exception e)
                    {
                        this.logger.LogDebug($"There are some issue reading sln in Destination: {this.Destination} exp message: {e.Message}");
                    }

                    // Get published nugets
                    try
                    {
                        proj.GetPublishedNugets();
                    }
                    catch (Exception e)
                    {
                        this.logger.LogDebug($"There are some issue reading sln in Destination: {this.Destination} exp message: {e.Message}");
                    }

                    // Get used nugets 
                    try
                    {
                        proj.GetUsedNugets();
                    }
                    catch (Exception e)
                    {
                        this.logger.LogDebug($"There are some issue reading sln in Destination: {this.Destination} exp message: {e.Message}");
                    }
                }

                // Get project dependendents
                // Get nuget dependencies
            }
            catch (Exception e)
            {
                //TODO
                this.logger.LogDebug($"There are some issue reading sln in Destination: {this.Destination} exp message: {e.Message}");
            }

        }
    }
}
