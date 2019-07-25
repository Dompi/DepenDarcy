using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DepenDarcy.Core.Entities
{
    public class Graph
    {
        private readonly ILogger logger;

        public List<Node> Nodes { get; private set; }


        
        public Graph(ILogger logger)
        {
            this.logger = logger;
            this.Nodes = new List<Node>();
        }

        public void BuildGraph(string root)
        {
            var Solutions = new List<Solution>();
            var Projects = new List<Project>();

            // Create and analyze solutions
            foreach (var currentFile in Directory.GetFiles(root, "*.sln", SearchOption.AllDirectories))
            {
                Solution s = new Solution(currentFile, this.logger);
                Solutions.Add(s);

                // Add graph points
                Projects.AddRange(s.Projects);
            }

            // Create dependencies throw nugets
            foreach (var proj in Projects.Where(x=>x.PublishedNugets.Any()))
            {
                foreach (var publishedNuget in proj.PublishedNugets)
                {
                    foreach (var testedProj in Projects)
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




        }

        public void GetDependencies(Node node)
        {
            List<Project> projects = new List<Project>();
            do
            {
                foreach (var item in node.Edges)
                {

                }
            } while (true);
        }
    }
}
