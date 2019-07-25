using System.Collections.Generic;

namespace DepenDarcy.Core.Entities
{
    public class Edge
    {
        public List<Node> PointTo { get; set; }

        public Edge()
        {
            this.PointTo = new List<Node>();
        }
    }
}
