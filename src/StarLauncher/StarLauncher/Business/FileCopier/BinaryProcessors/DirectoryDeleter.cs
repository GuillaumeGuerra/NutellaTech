using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;

namespace StarLauncher.Business
{
    [Export("DirectoryDelete", typeof(IBinaryProcessor))]
    public class DirectoryDeleter : AbstractBinaryProcessor<string>
    {
        protected override void ProcessItem(string item)
        {
            if (Directory.Exists(item))
                Directory.Delete(item, true);
        }

        protected override string GetErrorMessage(string item)
        {
            return string.Format("Unable to delete directory {0}", item);
        }

        protected override string GetProgressMessage(string item)
        {
            return string.Format("Deleting directory {0} ...", item);
        }
    }
}
