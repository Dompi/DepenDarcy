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
                        if (testedProj.ProjectDependencies.Any(y => y.Project.Name == proj.Name) == false)
                        {
                            testedProj.ProjectDependencies.Add( new ProjectDependency { Project = proj, ProjectDependencyType = ProjectDependencyType.Nuget });
                        }
                        if (proj.ProjectDependents.Any(y => y.Project.Name == testedProj.Name) == false)
                        {
                            proj.ProjectDependents.Add(new ProjectDependency { Project = testedProj, ProjectDependencyType = ProjectDependencyType.Nuget });
                        }
                    }
                }
            }
        }

        public Dictionary<int, List<Project>> GetDependencies(Project project)
        {
            int level = 1;
            Dictionary<int, List<Project>> graphLevels = new Dictionary<int, List<Project>>()
            {
                { 0, new List<Project> { project} }
            };
            List<ProjectDependency> projectsToScan = new List<ProjectDependency>(project.ProjectDependents);
            while (projectsToScan.Any())
            {
                List<ProjectDependency> nextLevel = new List<ProjectDependency>();
                foreach (var proj in projectsToScan)
                {
                    nextLevel.AddRange(proj.Dependents.Where(x=>nextLevel.Select(n=>n.Name).Contains(x.Name) == false));
                }
                graphLevels.Add(level, new List<Project>(projectsToScan));
                level++;
                projectsToScan = nextLevel.Where(x=>x.PublishedNugets.Any()).ToList();
            }


            return graphLevels;
        }
    }
}
