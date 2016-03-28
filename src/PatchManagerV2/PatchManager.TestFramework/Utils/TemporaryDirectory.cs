using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatchManager.TestFramework.Utils
{
    public class TemporaryDirectory : IDisposable
    {
        public string Location { get; set; }

        public TemporaryDirectory()
        {
            Location = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            if (Directory.Exists(Location))
                FileHelper.DeleteDirectory(Location);

            Directory.CreateDirectory(Location);
        }

        public void Dispose()
        {
            if (Directory.Exists(Location))
                FileHelper.DeleteDirectory(Location);
        }
    }
}
