using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;

namespace StarLauncher.Business
{
    [Export("DirectoryCreate", typeof(IBinaryProcessor))]
    public class DirectoryCreator : AbstractBinaryProcessor<string>
    {
        protected override void ProcessItem(string item)
        {
            Directory.CreateDirectory(item);
        }

        protected override string GetErrorMessage(string item)
        {
            return string.Format("Unable to create directory {0}", item);
        }

        protected override string GetProgressMessage(string item)
        {
            return string.Format("Creating directory {0} ...", item);
        }
    }
}
