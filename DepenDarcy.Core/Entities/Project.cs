using System;
using System.Collections.Generic;
using System.Text;

namespace DepenDarcy.Core.Entities
{
    class Project
    {
        public List<string> Dependencies { get; set; }
        public List<string> Dependents { get; set; }

        public Project()
        {
            this.Dependencies = new List<string>();
            this.Dependents = new List<string>();
                 
        }

    }
}
