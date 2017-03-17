using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DEEntities
{
    [DataContract]
    public class Pasta
    {
        [DataMember]
        public int Codigo { get; set; }

        [DataMember]
        public int CodigoPai { get; set; }

        [DataMember]
        public int CodigoDepto { get; set; }

        [DataMember]
        public int CdEntidade { get; set; }

        [DataMember]
        public string NmPasta { get; set; }

        [DataMember]
        public string DsPasta { get; set; }

        [DataMember]
        public DateTime DtCriacao { get; set; }

        [DataMember]
        public Entidade Owner { get; set; }

        [DataMember]
        public Pasta PastaPai { get; set; }
    }
}
