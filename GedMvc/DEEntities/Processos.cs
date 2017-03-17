using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DEEntities
{
    [DataContract]
    public class Processos
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int CdEntidade { get; set; }

        [DataMember]
        public string NomeProcesso { get; set; }

        [DataMember]
        public string Descricao { get; set; }

        [DataMember]
        public int IdTipo { get; set; }

        /// <summary>
        /// Pasta pai do processo
        /// </summary>
        [DataMember]
        public int IdPai { get; set; }

        [DataMember]
        public List<Repositorio> Arquivos { get; set; }

        [DataMember]
        public string PastaPai { get; set; }

        [DataMember]
        public Entidade Owner { get; set; }
    }
}
