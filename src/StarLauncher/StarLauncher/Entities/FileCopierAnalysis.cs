using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StarLauncher.Entities
{
    public class FileCopierAnalysis
    {
        public List<string> FilesToDelete { get; set; }
        public List<Tuple<string, string>> FilesToCreate { get; set; }
        public List<Tuple<FileInfo, FileInfo>> FilesToUpdate { get; set; }

        public List<string> DirectoriesToDelete { get; set; }
        public List<string> DirectoriesToCreate { get; set; }

        public FileCopierAnalysis()
        {
            FilesToDelete = new List<string>();
            FilesToCreate = new List<Tuple<string, string>>();
            FilesToUpdate = new List<Tuple<FileInfo, FileInfo>>();

            DirectoriesToDelete = new List<string>();
            DirectoriesToCreate = new List<string>();
        }
    }
}
