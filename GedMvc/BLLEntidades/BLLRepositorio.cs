using DAL;
using DEEntities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using Utilities;

namespace BLLEntidades
{
    public class BLLRepositorio
    {
        static DalRepositorio dalrepo = new DalRepositorio();

        private static int SalvarArquivo(Repositorio repositorio)
        {
            return dalrepo.Save(repositorio);
        }

        /// <summary>
        /// Insere um registro no banco de dados verificando se da suporte a extensão e se o arquivo ja nao existe no banco
        /// </summary>
        /// <param name="repositorio"></param>
        /// <returns></returns>
        public static StatusOperacao SalvarArquivoValidando(Repositorio repositorio)
        {
            StatusOperacao so = new StatusOperacao();

            //Caso o sistema não de suporte a extensão
            if (Util.GetMimeType(repositorio.Extensao) == "")
            {
                so.Msg = "Extensão do arquivo não suportada pelo sistema, contate o administrador";
                return so;
            }

            //verificando se o arquivo ja existe no diretorio, o que pode indicar um erro caso ele nao exista no banco
            //so.Msg = Util.CheckFileExists(repositorio.Caminho);

            if (so.Msg == null)
            {
                repositorio.CdRepositorio = dalrepo.Save(repositorio);
                so.Msg = repositorio.CdRepositorio > 0 ? null : "Registro não foi inserido";
            }
            so.Arquivo = repositorio;
            return so;
        }

        public static bool DeletarArquivo(int cdrepositorio, string nome)
        {
            bool ret = false;
            string path = "C:\\gedrepositorio\\" + nome;
            FileInfo file = new FileInfo(path);
            if (file.Exists) { file.Delete(); }

            ret = dalrepo.Delete(new Repositorio() { CdRepositorio = cdrepositorio });

            return ret;
        }

        public static IList<DEEntities.Repositorio> RetornarListaPorDeptoUsuario(string iddepto, string chave)
        {
            IList<DEEntities.Repositorio> lista = null;
            lista = dalrepo.RetornarListaPorDeptoUsuario(iddepto, chave);
            return lista;
        }
    }
}
