using StarLauncher.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;

namespace StarLauncher.Business
{
    [Export(typeof(IBinariesAnalyser))]
    public class BinariesAnalyser : IBinariesAnalyser
    {
        public FileCopierAnalysis AnalyseBinaries(StarEnvironment environment)
        {
            var analysis = new FileCopierAnalysis();

            AnalyseFiles(environment, analysis);
            AnalyseDirectories(environment, analysis);

            return analysis;
        }

        private void AnalyseFiles(StarEnvironment environment, FileCopierAnalysis analysis)
        {
            Dictionary<string, string> sourceFiles = GetAllFiles(environment.SourceDirectoryName);
            Dictionary<string, string> targetFiles = GetAllFiles(environment.TargetDirectoryName);

            foreach (var file in sourceFiles)
            {
                string targetFile = null;
                if (targetFiles.TryGetValue(file.Key, out targetFile))
                    analysis.FilesToUpdate.Add(new Tuple<FileInfo, FileInfo>(new FileInfo(file.Value), new FileInfo(targetFile)));
                else
                {
                    string targetPath = file.Value.Replace(environment.SourceDirectoryName, environment.TargetDirectoryName);
                    analysis.FilesToCreate.Add(new Tuple<string, string>(file.Value, targetPath));
                }
            }
            foreach (var file in targetFiles)
            {
                if (!sourceFiles.ContainsKey(file.Key))
                    analysis.FilesToDelete.Add(file.Value);
            }
        }

        private void AnalyseDirectories(StarEnvironment environment, FileCopierAnalysis analysis)
        {
            Dictionary<string, string> sourceDirectories = GetAllDirectories(environment.SourceDirectoryName);
            Dictionary<string, string> targetDirectories = GetAllDirectories(environment.TargetDirectoryName);

            foreach (var directory in sourceDirectories)
            {
                if (!targetDirectories.ContainsKey(directory.Key))
                    analysis.DirectoriesToCreate.Add(directory.Value.Replace(environment.SourceDirectoryName, environment.TargetDirectoryName));
            }
            foreach (var directory in targetDirectories)
            {
                if (!sourceDirectories.ContainsKey(directory.Key))
                    analysis.DirectoriesToDelete.Add(directory.Value);
            }
        }

        private Dictionary<string, string> GetAllFiles(string directory)
        {
            var results = new Dictionary<string, string>();

            foreach (var file in Directory.GetFiles(directory, "*", SearchOption.AllDirectories))
            {
                results.Add(file.Replace(directory, ""), file);
            }

            return results;
        }

        private Dictionary<string, string> GetAllDirectories(string directory)
        {
            var results = new Dictionary<string, string>();

            foreach (var subDirectory in Directory.GetDirectories(directory, "*", SearchOption.AllDirectories))
            {
                results.Add(subDirectory.Replace(directory, ""), subDirectory);
            }

            return results;
        }
    }
}
