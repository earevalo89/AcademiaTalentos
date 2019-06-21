using System.Runtime.Serialization;

namespace ElVinoDorado.Plugins
{
    [DataContract]
    public class Vino
    {
        [DataMember]
        public string wine { get; set; }
        [DataMember]
        public double? score { get; set; }
    }
}
