using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ResharperPluginTestProject
{
    [DataContract]
    public class DataContractClass : IObjectTreeSerializable
    {
        public DataContractClass(IObjectTree tree)
        {
            IntProperty = tree.Get<int>("IntProperty");
        }

        public void FillObjectTree(IObjectTree tree)
        {
            tree.Set<int>("IntProperty", IntProperty);
        }

        public void UpgradeObjectTree(IObjectTree tree)
        {
// TODO : write upgrade code
        }

        [DataMember]
        public int IntProperty { get; set; }
        [DataMember]
        public string StringProperty { get; set; }
        [DataMember]
        public List<List<string>> ListProperty { get; set; }
        [DataMember]
        private int _intField;
        [DataMember]
        private string _stringField;
    }
}