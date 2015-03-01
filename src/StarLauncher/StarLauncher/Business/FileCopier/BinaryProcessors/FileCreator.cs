using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;

namespace StarLauncher.Business
{
    [Export("Create",typeof(IBinaryProcessor))]
    public class FileCreator : AbstractBinaryProcessor<Tuple<string, string>>
    {
        protected override void ProcessItem(Tuple<string, string> item)
        {
            File.Copy(item.Item1, item.Item2);
        }

        protected override string GetErrorMessage(Tuple<string, string> item)
        {
            return string.Format("Unable to copy file {0}", item.Item1);
        }

        protected override string GetProgressMessage(Tuple<string, string> item)
        {
            return string.Format("Copying file {0} ...", item.Item1);
        }
    }
}
