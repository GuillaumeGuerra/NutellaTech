using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;

namespace StarLauncher.Business
{
    [Export("Update",typeof(IBinaryProcessor))]
    public class FileUpdater : AbstractBinaryProcessor<Tuple<FileInfo, FileInfo>>
    {
        protected override void ProcessItem(Tuple<FileInfo, FileInfo> item)
        {
            if (item.Item1.LastWriteTime != item.Item2.LastWriteTime || item.Item1.Length != item.Item2.Length)
                File.Copy(item.Item1.FullName, item.Item2.FullName, true);
        }

        protected override string GetErrorMessage(Tuple<FileInfo, FileInfo> item)
        {
            return string.Format("Unable to update file {0}", item.Item1.FullName);
        }

        protected override string GetProgressMessage(Tuple<FileInfo, FileInfo> item)
        {
            return string.Format("Updating file {0} ...", item.Item1.FullName);
        }

        protected override bool ShouldProcessItem(Tuple<FileInfo, FileInfo> item)
        {
            return item.Item1.LastWriteTime != item.Item2.LastWriteTime || item.Item1.Length != item.Item2.Length;
        }
    }
}
