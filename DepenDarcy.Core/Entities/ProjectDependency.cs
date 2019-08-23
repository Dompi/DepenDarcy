namespace DepenDarcy.Core.Entities
{
    public class ProjectDependency
    {
        public ProjectDependencyType ProjectDependencyType { get; set; }
        public Project Project { get; set; }
        public string Reason { get; set; }
    }
}
