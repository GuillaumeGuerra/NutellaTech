using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarLauncher.Entities
{
    public class StarEnvironment
    {
        public string Name { get; set; }
        public Uri Image { get; set; }
        public string ExecutablePath { get; set; }
        public string HomewareRoot { get; set; }
        public string TargetDirectoryName { get; set; }
        public string SourceDirectoryName { get; set; }
    }
}
