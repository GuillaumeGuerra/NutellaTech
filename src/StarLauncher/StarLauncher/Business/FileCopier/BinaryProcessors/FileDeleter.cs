using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;

namespace StarLauncher.Business
{
    [Export("Delete",typeof(IBinaryProcessor))]
    public class FileDeleter : AbstractBinaryProcessor<string>
    {
        protected override void ProcessItem(string item)
        {
            File.Delete(item);
        }

        protected override string GetErrorMessage(string item)
        {
            return string.Format("Unable to delete file {0}", item);
        }

        protected override string GetProgressMessage(string item)
        {
            return string.Format("Deleting file {0} ...", item);
        }
    }
}
