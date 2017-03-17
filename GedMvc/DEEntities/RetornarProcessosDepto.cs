using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEEntities
{
    public class RetornarProcessosDeptoView
    {
        public string NomeProcesso { get; set; }
        public string DescricaoProcesso { get; set; }
        public string Departamento { get; set; }
        public DateTime DataCriacao { get; set; }
        public string Status { get; set; }
    }
}
