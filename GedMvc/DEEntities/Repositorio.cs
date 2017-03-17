using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DEEntities
{
    [DataContract]
    public class Repositorio
    {
        [DataMember]
        public int CdRepositorio { get; set; }

        [DataMember]
        public int CdProcesso { get; set; }

        [DataMember]
        public int CdPasta { get; set; }

        [DataMember]
        public long Tamanho { get; set; }

        [DataMember]
        public int CdEntidade { get; set; }

        private string nomearquivo;
        /// <summary>
        /// Nome logico, ou nome original do arquivo
        /// </summary>
        [DataMember]
        public string NomeArquivo
        {
            get { return nomearquivo; }
            set { nomearquivo = value.Replace("'", ""); }
        }

        /// <summary>
        /// Nome que o arquivo é salvo no diretorio
        /// </summary>
        [DataMember]
        public string NomeFisicoArquivo { get; set; }

        [DataMember]
        public string Extensao
        {
            get;
            set;
        }

        [DataMember]
        public string Caminho { get; set; }

        [DataMember]
        public Entidade Owner { get; set; }

    }
}
