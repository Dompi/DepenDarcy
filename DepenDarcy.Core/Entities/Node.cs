using System.Collections.Generic;

namespace DepenDarcy.Core.Entities
{
    public class Node
    {
        public Project Project { get; set; }
        public List<Edge> Edges { get; set; }
    }
}
