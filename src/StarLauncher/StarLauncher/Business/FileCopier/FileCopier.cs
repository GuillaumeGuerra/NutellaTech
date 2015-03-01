using StarLauncher.Entities;
using StarLauncher.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;

namespace StarLauncher.Business
{
    [Export(typeof(IFileCopier))]
    public class FileCopier : IFileCopier
    {
        [Import]
        public IBinariesAnalyser Analyser { get; set; }

        [Import("Delete")]
        public IBinaryProcessor FileDeleter { get; set; }
        [Import("Update")]
        public IBinaryProcessor FileUpdater { get; set; }
        [Import("Create")]
        public IBinaryProcessor FileCreator { get; set; }
        [Import("DirectoryCreate")]
        public IBinaryProcessor DirectoryCreator { get; set; }
        [Import("DirectoryDelete")]
        public IBinaryProcessor DirectoryDeleter { get; set; }

        public void CopyFiles(StarEnvironment environment, IObserver observer)
        {
            CheckDirectory(Path.Combine(environment.HomewareRoot, environment.TargetDirectoryName));

            var analysis = Analyser.AnalyseBinaries(environment);
            DirectoryCreator.ProcessBinaries(analysis.DirectoriesToCreate, observer);
            FileDeleter.ProcessBinaries(analysis.FilesToDelete, observer);
            FileUpdater.ProcessBinaries(analysis.FilesToUpdate, observer);
            FileCreator.ProcessBinaries(analysis.FilesToCreate, observer);
            DirectoryDeleter.ProcessBinaries(analysis.DirectoriesToDelete, observer);
        }

        private void CheckDirectory(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            //TODO : check whether the directory is locked for editing ...
        }
    }
}
