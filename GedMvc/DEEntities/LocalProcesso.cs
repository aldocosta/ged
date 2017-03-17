using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DEEntities
{
    [DataContract]
    public class LocalProcesso
    {
        [DataMember]
        public int IdLocalProcesso { get; set; }

        [DataMember]
        public DateTime DataCriacao { get; set; }

        [DataMember]
        public int IdProcesso { get; set; }

        [DataMember]
        public int IdDepto { get; set; }
    }
}
