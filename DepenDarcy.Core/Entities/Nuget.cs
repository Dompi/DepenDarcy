using System.Collections.Generic;

namespace DepenDarcy.Core.Entities
{
    public class Nuget
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public List<Dependency> Dependencies { get; set; }
    }
}
