using System.Runtime.Serialization;

namespace EnzymePackageRegistration
{
    [DataContract(Name = "component")]
    public class Components
    {
        [DataMember(Name = "component")]
        public Component component;

        [DataMember(Name = "ms.internal")]
        public bool internaluse;

        [DataMember(Name = "ms.transmitted")]
        public bool transmitted;

        [DataMember(Name = "result")]
        public Result result;
    }
}
