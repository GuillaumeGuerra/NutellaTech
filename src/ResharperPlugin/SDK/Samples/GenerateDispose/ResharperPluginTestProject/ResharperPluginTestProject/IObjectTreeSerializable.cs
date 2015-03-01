using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResharperPluginTestProject
{
    public interface IObjectTreeSerializable
    {
        void FillObjectTree(IObjectTree tree);
        void UpgradeObjectTree(IObjectTree tree);
    }

    public interface IObjectTree
    {
        T Get<T>(string name);
        void Set<T>(string name, T value);
    }
}
