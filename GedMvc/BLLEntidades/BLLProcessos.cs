using DEEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLLEntidades
{
    public class BLLProcessos
    {
        /// <summary>
        /// Retorna um processo pelo seu ID
        /// </summary>
        /// <param name="idprocesso"></param>
        /// <returns></returns>
        public static Processos RetornarProcesso(int idprocesso)
        {
            DAL.DALProcesso dalproc = new DAL.DALProcesso();
            Processos proc = dalproc.RetornarEntidade(idprocesso);
            
            return proc;
        }


        public static IList<Processos> RetornarListaProcesso()
        {
            DAL.DALProcesso dalproc = new DAL.DALProcesso();
            var proc = dalproc.RetornarLista();

            return proc;
        }

    }
}
