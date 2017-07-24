using System.Runtime.Serialization;

namespace EnzymePackageRegistration
{
    [DataContract(Name = "component")]
    public class Component
    {
        [DataMember(Name = "npmjs.org")]
        public Npm npm;
    }
}
