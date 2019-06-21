using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ElVinoDorado.Plugins
{
    // A DataContract is a formal agreement between a service and a client
    // that abstractly describes the data to be exchanged. That is, to communicate,
    // the client and the service do not have to share the same types, 
    // only the same data contracts.
    [DataContract]
    public class Vinos
    {
        [DataMember]
        public List<Vino> results { get; set; }
        
    }
}
