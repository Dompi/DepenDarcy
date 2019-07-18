using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;

namespace DepenDarcy.Core.Entities
{
    public class Graph
    {
        private List<Solution> solutions;
        private readonly string root;
        private readonly ILogger logger;

        public Graph(string root, ILogger logger)
        {
            this.root = root;
            this.logger = logger;
            this.solutions = new List<Solution>();
        }

        public void BuildGraph()
        {
            foreach (var currentFile in Directory.GetFiles(root, "*.sln", SearchOption.AllDirectories))
            {
                Solution s = new Solution(currentFile, this.logger);
                s.Analyze();
                solutions.Add(s);
            }

        }
    }
}
