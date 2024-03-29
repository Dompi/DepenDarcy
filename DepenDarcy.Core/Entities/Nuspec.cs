﻿using System.Collections.Generic;

namespace DepenDarcy.Core
{
    public class Nuspec
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Owners { get; set; }
        public string LicenseUrl { get; set; }
        public string ProjectUrl { get; set; }
        public string IconUrl { get; set; }
        public string RequireLicenseAcceptance { get; set; }
        public string Description { get; set; }
        public string ReleaseNotes { get; set; }
        public string Copyright { get; set; }
        public string Tags { get; set; }
        public List<NugetDependency> Dependencies { get; set; }
    }
}
