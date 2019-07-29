using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DepenDarcy.Core.Entities
{
    public class Graph
    {
        private readonly ILogger logger;

        public List<Solution> Solutions;
        public List<Project> Projects;

        public Graph(ILogger logger)
        {
            this.logger = logger;
            this.Solutions = new List<Solution>();
            this.Projects = new List<Project>();
        }

        public void BuildGraph(string root)
        {
            // Create and analyze solutions
            foreach (var currentFile in Directory.GetFiles(root, "*.sln", SearchOption.AllDirectories))
            {
                Solution s = new Solution(currentFile, this.logger);
                Solutions.Add(s);

                // Add graph points
                Projects.AddRange(s.Projects);
            }

            // Create dependencies throw nugets
            foreach (var proj in Projects.Where(x => x.PublishedNugets.Any()))
            {
                foreach (var publishedNuget in proj.PublishedNugets)
                {
                    foreach (var testedProj in Projects.Where(x => x.UsedNugets.Select(y => y.Name).Contains(publishedNuget.Name)))
                    {
                        if (testedProj.Dependencies.Any(y => y.Name == proj.Name) == false)
                        {
                            testedProj.Dependencies.Add(proj);
                        }
                        if (proj.Dependents.Any(y => y.Name == testedProj.Name) == false)
                        {
                            proj.Dependents.Add(testedProj);
                        }
                    }
                }
            }
        }

        public void GetDependencies(Project project)
        {
            int level = 1;
            Dictionary<int, List<Project>> graphLevels = new Dictionary<int, List<Project>>()
            {
                { 0, new List<Project> { project} }
            };
            List<Project> projectsToScan = new List<Project>(project.Dependents);
            while (projectsToScan.Any())
            {
                List<Project> nextLevel = new List<Project>();
                foreach (var proj in projectsToScan)
                {
                    nextLevel.AddRange(proj.Dependents);
                }
                graphLevels.Add(level, new List<Project>(projectsToScan));
                level++;
                projectsToScan = nextLevel;
            }
        }
    }
}
