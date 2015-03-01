using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResharperPluginTestProject
{
    [Serializable]
    public class SerializableClass : IObjectTreeSerializable
    {
        public int IntProperty { get; set; }

        public string StringProperty { get; set; }
        public List<List<string>> ListProperty { get; set; }
        private int _intField;

        public SerializableClass(IObjectTree tree)
        {
            _intField = tree.Get<int>("_intField");
            _stringField = tree.Get<string>("_stringField");
            IntProperty = tree.Get<int>("IntProperty");
        }

        public void FillObjectTree(IObjectTree tree)
        {
            tree.Set<int>("_intField", _intField);
            tree.Set<string>("_stringField", _stringField);
            tree.Set<int>("IntProperty", IntProperty);
        }

        public void UpgradeObjectTree(IObjectTree tree)
        {
            // TODO : write upgrade code
        }

        private string _stringField;
    }
}
