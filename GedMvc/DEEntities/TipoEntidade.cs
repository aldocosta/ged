using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DEEntities
{
    [DataContract]
    public class TipoEntidade
    {
        [DataMember]
        public int Codigo { get; set; }
        
        [DataMember]
        public string Nome { get; set; }
    }
}
