using System.Collections.Generic;

namespace DepenDarcy.Core.Entities
{
    public class Nuget
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string PublishingProject { get; set; }
        public List<string> Dependencies { get; set; }
    }
}
