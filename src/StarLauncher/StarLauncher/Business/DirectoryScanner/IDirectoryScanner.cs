﻿using StarLauncher.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarLauncher.Business
{
    public interface IDirectoryScanner
    {
        StarEnvironment ScanDirectory(string directory);
    }
}
