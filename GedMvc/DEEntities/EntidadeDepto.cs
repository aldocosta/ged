using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DEEntities
{
    [DataContract]
    public class EntidadeDepto
    {
        [DataMember]
        public int cdEntidade { get; set; }
        
        [DataMember]
        public int cdDepto { get; set; }

        [DataMember]
        public string NmEntidade { get; set; }

        [DataMember]
        public string NmDepto { get; set; }
    }
}
