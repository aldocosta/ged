using DEEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLLEntidades
{
    public class BLLEntidadeDepto
    {
        static DAL.DALEntidadeDepto daled = new DAL.DALEntidadeDepto();

        public static bool DeletarRelacao(string identidade, string iddepto)
        {
            return daled.Delete(new EntidadeDepto()
            {
                cdDepto = int.Parse(iddepto),
                cdEntidade = int.Parse(identidade)
            });            
        }

        public static int AtribuirEntidadeDepto(Entidade e, Depto d)
        {
            EntidadeDepto ed = new EntidadeDepto();
            ed.cdEntidade = e.Codigo;
            ed.cdDepto = d.Id;

            return daled.Save(ed);
        }

        public static IList<EntidadeDepto> RetornarDepartamentosEntidade(EntidadeDepto ed)
        {
            return daled.RetornarLista(ed);
        }


    }
}
