using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DEEntities
{
    [DataContract]
    public class Entidade
    {        
        [DataMember]
        public int Codigo { get; set; }
        
        [DataMember]
        public string NmEntidade { get; set; }

        [DataMember]
        public string NmUser { get; set; }

        [DataMember]
        public string NmPass { get; set; }
        [DataMember]
        public string NmEmail { get; set; }        

        [DataMember]
        public int CodigoEntidadePai { get; set; }

        [DataMember]
        public int Isdeletado { get; set; }

        [DataMember]
        public TipoEntidade Tipo { get; set; }

        [DataMember]
        public Entidade EntidadePai { get; set; }        

        /// <summary>
        /// Traz a base selecionada pelo usuário
        /// </summary>
        [DataMember]
        public string BaseAtual { get; set; }


    }
}
