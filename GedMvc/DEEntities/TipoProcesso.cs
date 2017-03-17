using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DEEntities
{
    [DataContract]
    public class TipoProcesso
    {
        [DataMember]
        public int IdProcesso { get; set; }

        [DataMember]
        public string NomeProcesso { get; set; }
    }
}
