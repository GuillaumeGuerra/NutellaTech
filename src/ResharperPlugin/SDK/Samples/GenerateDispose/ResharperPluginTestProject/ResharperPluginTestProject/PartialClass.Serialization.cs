using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResharperPluginTestProject
{
    public partial class PartialClass
    {
        public PartialClass(IObjectTree tree)
        {
            MyAssProperty = tree.Get<double>("MyAssProperty");
            StringProperty = tree.Get<string>("StringProperty");
        }

        public void FillObjectTree(IObjectTree tree)
        {
            tree.Set<double>("MyAssProperty", MyAssProperty);
            tree.Set<string>("StringProperty", StringProperty);
        }

        public void UpgradeObjectTree(IObjectTree tree)
        {
// TODO : write upgrade code
        }

        public DateTime DateTimeProperty { get; set; }

        private void TestMethod()
        {
            
        }
    }
}
