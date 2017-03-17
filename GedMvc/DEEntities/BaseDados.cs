using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DEEntities
{
    [DataContract]
    public class BaseDados
    {
        [DataMember]
        public int CdBase { get; set; }
        
        [DataMember]
        public int CdEntidadeOwner { get; set; }

        [DataMember]
        public string Caminho { get; set; }

        [DataMember]
        public string NomeBase { get; set; }

    }
}
