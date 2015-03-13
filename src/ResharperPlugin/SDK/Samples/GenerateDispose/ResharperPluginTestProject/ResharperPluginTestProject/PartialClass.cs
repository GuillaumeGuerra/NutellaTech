using System;

namespace ResharperPluginTestProject
{
    [Serializable]
    public partial class PartialClass
    {
        public string StringProperty { get; set; }
    }

    public partial class PartialClass
    {
        public double MyAssProperty { get; set; } 
    }
}