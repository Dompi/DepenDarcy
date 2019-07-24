using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DepenDarcy.Core.Entities
{
    public class Graph
    {
        public List<Solution> Solutions { get; private set; }
        public List<Project> Projects { get; private set; }
        private readonly string root;
        private readonly ILogger logger;

        public Graph(string root, ILogger logger)
        {
            this.root = root;
            this.logger = logger;
            this.Solutions = new List<Solution>();
            this.Projects = new List<Project>();
        }

        public void BuildGraph()
        {
            // Create and analyze solutions
            foreach (var currentFile in Directory.GetFiles(root, "*.sln", SearchOption.AllDirectories))
            {
                Solution s = new Solution(currentFile, this.logger);
                Solutions.Add(s);

                // Add graph points
                this.Projects.AddRange(s.Projects);
            }

            // Create dependencies throw nugets
            foreach (var proj in this.Projects.Where(x=>x.PublishedNugets.Any()))
            {
                foreach (var publishedNuget in proj.PublishedNugets)
                {
                    foreach (var testedProj in this.projects)
                    {
                        if (testedProj.UsedNugets.Select(x=>x.Name).Contains(publishedNuget.Name) == true &&
                            testedProj.Dependencies.Any(y => y.Name == proj.Name) == false)
                        {
                            testedProj.Dependencies.Add(proj);
                        }
                    }
                }
            }

            // Create dependency graph edges



            var d = this.Projects.Single(x => x.Name.Equals("Customers.Core")).Dependencies;
            var p = this.Projects.Single(x => x.Name.Equals("Customers.Core")).PublishedNugets;
            var u = this.Projects.Single(x => x.Name.Equals("Customers.Core")).UsedNugets;

            var aaaa = 3;
        }

        public void GetDependencies(Project project)
        {
            var p = this.Projects.Single(x => x.Name == project.Name);
            do
            {

            } while (true);
        }
    }
}
