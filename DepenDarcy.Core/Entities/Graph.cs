using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;

namespace DepenDarcy.Core.Entities
{
    public class Graph
    {
        private List<Solution> solutions;
        private List<Project> projects;
        private readonly string root;
        private readonly ILogger logger;

        public Graph(string root, ILogger logger)
        {
            this.root = root;
            this.logger = logger;
            this.solutions = new List<Solution>();
            this.projects = new List<Project>();
        }

        public void BuildGraph()
        {
            // Create and analyze solutions
            foreach (var currentFile in Directory.GetFiles(root, "*.sln", SearchOption.AllDirectories))
            {
                Solution s = new Solution(currentFile, this.logger);
                solutions.Add(s);

                // Add graph points
                this.projects.AddRange(s.Projects);
            }

            // Create dependency graph edges
            foreach (var proj in this.projects)
            {

            }
        }
    }
}
