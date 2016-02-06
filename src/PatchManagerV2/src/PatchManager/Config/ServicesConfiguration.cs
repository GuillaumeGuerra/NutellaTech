using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatchManager.Config
{
    public class ServicesConfiguration
    {
        public SingleServiceConfiguration Persistence { get; set; }

        public SingleServiceConfiguration Model { get; set; }

        public SingleServiceConfiguration Gerrit { get; set; }

        public SingleServiceConfiguration Jira { get; set; }

        public SingleServiceConfiguration Resolver { get; set; }
    }
}
