using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{

    [Serializable]
    public class Configuracao
    { 
        public string NomeCliente { get; set; }
        public string IdCliente { get; set; }
        public DateTime DataInicioLicensa { get; set; }
        public DateTime DataFimLicensa { get; set; }
        public string GuidValue { get; set; }
    }
}
