using DAL;
using DEEntities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Utilities;

namespace BLLEntidades
{
    public class BLLPasta
    {
        static DalPasta dale = new DalPasta();
        static DALDepto daledepto = new DALDepto();

        public static int SalvarPasta(Pasta p)
        {
            return dale.Save(p);
        }

        public static bool DeletarPasta(Pasta p)
        {
            return dale.Delete(p);
        }

        public static bool AtualizarPasta(Pasta p)
        {
            return dale.Update(p);
        }

        public static Depto RetornarDepto(int iddepto)
        {
            return daledepto.RetornarEntidade(iddepto);
        }

        public static Pasta RetornarPasta(int id)
        {
            return dale.RetornarEntidade(id);
        }

        public static IList<Pasta> RetornarPastasDepto(int iddepto)
        {
            var ret = dale.RetornarLista().Where(p => p.CodigoDepto == iddepto).ToList();
            return ret;
        }

        public static IList<Pasta> RetornarPastas()
        {
            var ret = dale.RetornarLista();
            return ret;
        }

        public static IList<Pasta> RetornarPastasPastapai(int idpai)
        {
            return dale.RetornarLista().Where(p => p.CodigoPai == idpai).ToList();
        }
    }
}
