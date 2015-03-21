using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResharperPluginTestProject
{
    [Serializable]
    public partial class OrdinaryClass
    {
        public int IntProperty { get; set; }

        public string StringProperty { get; set; }
        public List<List<string>> ListProperty { get; set; }
        private int _intField;
    }
}
