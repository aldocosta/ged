using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GedOff.ViewModels
{
    public class EntidadeDeptos
    {
        public List<DEEntities.Entidade> ListEntidade { get; set; }
        public List<DEEntities.Depto> ListDeptos { get; set; }
    }
}