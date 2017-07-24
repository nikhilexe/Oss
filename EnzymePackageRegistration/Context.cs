using System.Runtime.Serialization;

namespace EnzymePackageRegistration
{
    [DataContract(Name = "context")]
    public class Context
    {
        [DataMember(Name = "user")]
        public string user;

        [DataMember(Name = "ms.group")]
        public string group;

        [DataMember(Name = "ms.project")]
        public string project;
    }
}
