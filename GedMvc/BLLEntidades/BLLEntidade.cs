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
    public class BLLEntidade
    {
        static DAL.DALEntidade dale = new DALEntidade();

        /// <summary>
        /// Realiza o logon do usuario
        /// </summary>
        /// <param name="login">Nome de login</param>
        /// <param name="pass">Password</param>
        /// <returns>Retorna um usuario logado wrapped em um objeto Entidade</returns>
        public static Entidade Logar(string login, string pass)
        {
            Entidade entidade = dale.Logar(login, pass);
            return entidade;
        }

        public static Entidade RetornarEntidade(int id)
        {
            Entidade entidade = dale.RetornarEntidade(id);
            return entidade;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>


        /// <summary>
        /// Grava entidades quando a entidade esta com uma entidade pai atribuida
        /// </summary>
        /// <param name="e">Entidade a gravar</param>
        /// <returns>Entidade gravada</returns>
        public static Entidade GravarEntidade(Entidade entidadeLogada, Entidade entidadeDestino)
        {
            if (entidadeDestino.NmUser == "admin")
            {
                throw new Exception("Nome de usuário não permitido!");
            }
            if (entidadeLogada.Tipo.Nome != "Administrador")
            {
                throw new Exception("Apenas administrador pode gravar novos usuários");
            }
            if (entidadeDestino.NmEntidade == "")
            {
                throw new Exception("Campo obrigatório (NOME DA ENTIDADE)");
            }
            if (entidadeDestino.NmUser == "")
            {
                throw new Exception("Campo obrigatório (NOME DO USUÁRIO)");
            }
            if (entidadeDestino.NmEmail == "")
            {
                throw new Exception("Campo obrigatório (EMAIL DO USUÁRIO)");
            }
            entidadeDestino.Codigo = dale.Save(entidadeDestino);
            return entidadeDestino;
        }

        public static Entidade GravarEntidadePai(Entidade e)
        {
            if (e.CodigoEntidadePai == 0)
            {
                e.Codigo = dale.Save(e);
            }
            return e;
        }


        /// <summary>
        /// Deleta entidades que não sejam entidade pai
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool DeletarEntidade(Entidade e)
        {
            e.Isdeletado = 1;
            //return dale.MarkAsDeleted(e);            
            return dale.Delete(e);
        }

        /// <summary>
        /// Deleta entidades que não sejam entidade pai
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool MarkAsDeleted(Entidade e)
        {
            e = dale.RetornarEntidade(e.Codigo);
            e.Isdeletado = 1;
            if (e.Tipo.Nome == "Administrador")
            {
                throw new Exception("Atenção, não é possível marcar o usuário admin como bloqueado");
            }
            return dale.MarkAsDeleted(e);                        
        }

        public static bool UnMarkUser(Entidade e)
        {
            e = dale.RetornarEntidade(e.Codigo);
            e.Isdeletado = 0;
            if (e.Tipo.Nome == "Administrador")
            {
                throw new Exception("Atenção, não é possível marcar o usuário admin como bloqueado");
            }
            return dale.Update(e);
        }

        /// <summary>
        /// Atualiaza uma entidade
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool AtualizarEntidade(Entidade e)
        {
            return dale.Update(e);
        }

        public static bool AtualizarEntidadePass(Entidade e)
        {
            return dale.UpdatePass(e);
        }

        public static IList<Entidade> RetornarListaEntidade()
        {
            return dale.RetornarLista().Where(p => p.Isdeletado == 0).ToList();
        }

        public static IList<Entidade> RetornarListaEntidadeMarcadosDeletado()
        {
            return dale.RetornarLista().Where(p => p.Isdeletado == 1).ToList();
        }

    }
}
