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
                            testedProj.ProjectDependencies.Add(new ProjectDependency { Project = proj, ProjectDependencyType = ProjectDependencyType.Nuget });
                        }
                    }
                }
            }
        }
        public Dictionary<int, List<Project>> GetNugetDependenciesDFS(Project project)
        {
            int level = 1;
            Dictionary<int, List<Project>> graphLevels = new Dictionary<int, List<Project>>()
            {
                { 0, new List<Project> { project} }
            };
            List<ProjectDependency> projectsDependencyToScan = new List<ProjectDependency>(project.ProjectDependents);

            while (projectsDependencyToScan.Any())
            {
                List<ProjectDependency> nextLevel = new List<ProjectDependency>();
                foreach (var dependency in projectsDependencyToScan)
                {
                    nextLevel.AddRange(dependency.Project.ProjectDependents.Where(x => nextLevel.Select(n => n.Project.Name).Contains(x.Project.Name) == false));
                }

                var nextLevelCandidates = projectsDependencyToScan.Where(d => d.ProjectDependencyType == ProjectDependencyType.Nuget).Select(x => x.Project);
                if (nextLevelCandidates.Any())
                {
                    graphLevels.Add(level, new List<Project>(nextLevelCandidates));
                }
                level++;
                projectsDependencyToScan = nextLevel.ToList();
            }

            return graphLevels;
        }
        public Dictionary<string, List<ProjectNugetVersion>> GetNugetProject()
        {
            Dictionary<string, List<ProjectNugetVersion>> projectNugetVersion = new Dictionary<string, List<ProjectNugetVersion>>();
            foreach (var proj in this.Projects)
            {
                foreach (var nuget in proj.UsedNugets)
                {
                    if (projectNugetVersion.TryGetValue(nuget.Name, out List<ProjectNugetVersion> projectList) == true)
                    {
                        projectList.Add(new ProjectNugetVersion { ProjectName = proj.Name, NugetVersion = nuget.Version });
                    }
                    else
                    {
                        projectNugetVersion.Add(nuget.Name, new List<ProjectNugetVersion>() { new ProjectNugetVersion { ProjectName = proj.Name, NugetVersion = nuget.Version } });
                    }
                }
            }

            return projectNugetVersion;
        }
        public List<List<Project>> GetBoundydContexts()
        {
            List<List<Project>> boundydContexts = new List<List<Project>>();
            List<Project> allProjects = new List<Project>(this.Projects);

            while (allProjects.Any())
            {
                List<Project> boundeydContext = new List<Project>();
                foreach (var item in this.GetDependenciesDFS(allProjects.First()))
                {
                    boundeydContext.AddRange(item.Value);
                    allProjects.RemoveAll(x=> item.Value.Select(s=>s.Name).Contains(x.Name));
                }
                boundydContexts.Add(boundeydContext);
            }

            return boundydContexts;
        }

        private Dictionary<int, List<Project>> GetDependenciesDFS(Project project)
        {
            int level = 1;
            Dictionary<int, List<Project>> graphLevels = new Dictionary<int, List<Project>>()
            {
                { 0, new List<Project> { project} }
            };
            List<ProjectDependency> projectsDependencyToScan = new List<ProjectDependency>(project.ProjectDependents);

            while (projectsDependencyToScan.Any())
            {
                List<ProjectDependency> nextLevel = new List<ProjectDependency>();
                foreach (var dependency in projectsDependencyToScan)
                {
                    nextLevel.AddRange(dependency.Project.ProjectDependents.Where(x => nextLevel.Select(n => n.Project.Name).Contains(x.Project.Name) == false));
                }

                var nextLevelCandidates = projectsDependencyToScan.Select(x => x.Project);
                if (nextLevelCandidates.Any())
                {
                    graphLevels.Add(level, new List<Project>(nextLevelCandidates));
                }
                level++;
                projectsDependencyToScan = nextLevel.ToList();
            }

            return graphLevels;
        }
    }
}
