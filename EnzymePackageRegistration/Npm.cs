using System.Runtime.Serialization;

namespace EnzymePackageRegistration
{
    [DataContract(Name = "npm")]
    public class Npm
    {
        [DataMember(Name = "name")]
        public string name;
        [DataMember(Name = "version")]
        public string version;
    }
}
