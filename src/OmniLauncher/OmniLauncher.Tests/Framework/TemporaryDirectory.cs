using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniLauncher.Tests.Framework
{
    public class TemporaryDirectory : IDisposable
    {
        public string Location { get; set; }

        public TemporaryDirectory()
        {
            Location = $"Data/{Guid.NewGuid()}";
            Directory.CreateDirectory(Location);
        }

        public void Dispose()
        {
            Directory.Delete(Location, true);
        }
    }
}
