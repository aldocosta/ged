using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DEEntities
{
    [DataContract]
    public class Depto
    {
        [DataMember]
        public int Id { get; set; }
        
        [DataMember]
        public int CdEntidade { get; set; }

        [DataMember]
        public string NomeDepto { get; set; }

        [DataMember]
        public DateTime DataCriacao { get; set; }

        [DataMember]
        public string OwnerName { get; set; }

        [DataMember]
        public Entidade Owner { get; set; }
    }
}
