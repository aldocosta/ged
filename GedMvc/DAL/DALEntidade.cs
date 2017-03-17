using DEEntities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Utilities;

namespace DAL
{
    public class DALEntidade : IIUD<Entidade>
    {
        public int Save(Entidade t)
        {
            bool isChild = t.CodigoEntidadePai != 0;
            int ret = 0;
            string campos = "nmEntidade,nmUserEntidade,nmPass,nmEmail,cdEntidadeTipo";
            string camposvalores = "'" + t.NmEntidade + "','" + t.NmUser + "','" + t.NmPass + "','" + t.NmEmail + "','" + t.Tipo.Codigo + "'";

            if (isChild)
            {
                campos += ",cdEntidadePai";
                camposvalores += "," + t.CodigoEntidadePai + "";
            }

            string queryInsert = "insert into TbEntidade (" + campos + ")";

            queryInsert += "values(" + camposvalores + "); select LAST_INSERT_ID() ";

            ret = Conexao.RealizarIUD_LastIdentifier(new MySqlDataAdapter(queryInsert, Conexao.Instance.StringConexao));
            t.Codigo = ret;
            return ret;
        }
        public bool Delete(Entidade t)
        {
            string query = "delete from TbEntidade where cdEntidade = " + t.Codigo;

            int execRet = Conexao.RealizarIUD(new MySqlDataAdapter(query, Conexao.Instance.StringConexao));

            return execRet > 0;
        }
        public bool Update(Entidade t)
        {
            string query = "update TbEntidade set nmEntidade='" + t.NmEntidade + "',nmUserEntidade='" + t.NmUser +
                "',nmEmail='" + t.NmEmail + "',cdEntidadeTipo=" + t.Tipo.Codigo + ",isdeletado=" + t.Isdeletado + " where cdEntidade =" + t.Codigo;

            int execRet = Conexao.RealizarIUD(new MySqlDataAdapter(query, Conexao.Instance.StringConexao));

            return execRet > 0;
        }

        public bool MarkAsDeleted(Entidade t)
        {
            string query = "update TbEntidade set isdeletado=" + t.Isdeletado + " where cdEntidade =" + t.Codigo + " ";

            int execRet = Conexao.RealizarIUD(new MySqlDataAdapter(query, Conexao.Instance.StringConexao));

            return execRet > 0;
        }

        public bool UpdatePass(Entidade t)
        {
            string query = "update TbEntidade set nmPass='" + t.NmPass + "' where cdEntidade =" + t.Codigo;

            int execRet = Conexao.RealizarIUD(new MySqlDataAdapter(query, Conexao.Instance.StringConexao));

            return execRet > 0;

        }

        public IList<Entidade> RetornarLista()
        {
            IList<Entidade> ret = new List<Entidade>();
            string query = "select * from TbEntidade e join TbTipoEntidade t on e.cdEntidadeTipo = t.cdTipoEntidade;";
            using (ConexaoGenerica con = new ConexaoGenerica(new MySqlDataAdapter(query, Conexao.Instance.StringConexao), false))
            {
                DbDataReader leitor = con.RetornarAdaptador().SelectCommand.ExecuteReader();
                while (leitor.Read())
                {
                    Entidade entidade = new Entidade();
                    entidade.Codigo = int.Parse(leitor[0].ToString());
                    entidade.NmEntidade = leitor[1].ToString();
                    entidade.NmUser = leitor[2].ToString();
                    entidade.NmPass = leitor[3].ToString();
                    entidade.NmEmail = leitor[4].ToString();

                    int temp = 0;
                    int.TryParse(leitor[7].ToString(), out temp);
                    entidade.Isdeletado = temp;

                    entidade.Tipo = new TipoEntidade()
                    {
                        Codigo = int.Parse(leitor[8].ToString()),
                        Nome = leitor[9].ToString()
                    };

                    ret.Add(entidade);
                }
                Util.FinalizarDataReader(leitor);
            }
            return ret;
        }
        public IList<Entidade> RetornarLista(Entidade t)
        {
            IList<Entidade> ret = new List<Entidade>();
            string query = "select * from TbEntidade where cdEntidade =" + t.Codigo;
            using (ConexaoGenerica con = new ConexaoGenerica(new MySqlDataAdapter(query, Conexao.Instance.StringConexao), false))
            {
                DbDataReader leitor = con.RetornarAdaptador().SelectCommand.ExecuteReader();
                while (leitor.Read())
                {
                    Entidade entidade = new Entidade();
                    entidade.Codigo = int.Parse(leitor[0].ToString());
                    entidade.NmEntidade = leitor[1].ToString();
                    entidade.NmUser = leitor[2].ToString();
                    entidade.NmPass = leitor[3].ToString();
                    entidade.NmEmail = leitor[4].ToString();
                    entidade.CodigoEntidadePai = int.Parse(leitor[6].ToString());
                    ret.Add(entidade);
                }
                Util.FinalizarDataReader(leitor);
            }
            return ret;
        }

        public Entidade Logar(string user, string pass)
        {
            Entidade entidade = null;
            using (ConexaoGenerica con = new ConexaoGenerica(new MySqlDataAdapter("Logar", Conexao.Instance.StringConexao), false))
            {
                MySqlCommand mcom = con.RetornarAdaptador().SelectCommand as MySqlCommand;
                mcom.Parameters.AddWithValue("login", user);
                mcom.Parameters.AddWithValue("pass", pass);
                mcom.CommandType = System.Data.CommandType.StoredProcedure;
                DbDataReader leitor = mcom.ExecuteReader();

                string queryTipo = "";

                if (leitor.Read())
                {
                    entidade = new Entidade();
                    entidade.Codigo = int.Parse(leitor[0].ToString());
                    entidade.NmEntidade = leitor[1].ToString();
                    entidade.NmUser = leitor[2].ToString();
                    entidade.NmEmail = leitor[4].ToString();
                    //queryTipo = "select * from TbTipoEntidade where cdTipoEntidade =" + leitor[5].ToString();
                    queryTipo = "select * from TbTipoEntidade where cdTipoEntidade =" + leitor[5].ToString();

                    if (leitor[6].ToString() != "")
                        entidade.CodigoEntidadePai = int.Parse(leitor[6].ToString());
                }
                Util.FinalizarDataReader(leitor);

                mcom.CommandType = System.Data.CommandType.Text;
                mcom.CommandText = queryTipo;
                if (queryTipo != null && queryTipo != "")
                {
                    leitor = mcom.ExecuteReader();

                    if (leitor.Read())
                    {
                        entidade.Tipo = new TipoEntidade()
                        {
                            Codigo = int.Parse(leitor[0].ToString()),
                            Nome = leitor[1].ToString()
                        };
                    }
                    Util.FinalizarDataReader(leitor);
                }
            }

            return entidade;
        }


        public Entidade RetornarEntidade(int id)
        {
            string query = "select * from TbEntidade where cdEntidade=" + id;
            Entidade entidade = null;
            using (ConexaoGenerica con = new ConexaoGenerica(new MySqlDataAdapter(query, Conexao.Instance.StringConexao), false))
            {
                DbDataReader leitor = con.RetornarAdaptador().SelectCommand.ExecuteReader();
                string queryTipo = "";
                if (leitor.Read())
                {
                    entidade = new Entidade();
                    entidade.Codigo = Convert.ToInt32(leitor[0]);
                    entidade.NmEntidade = leitor[1].ToString();
                    entidade.NmUser = leitor[2].ToString();
                    entidade.NmEmail = leitor[4].ToString();
                    queryTipo = "select * from TbTipoEntidade where cdTipoEntidade =" + leitor[5].ToString();
                }
                Utilities.Util.FinalizarDataReader(leitor);


                if (queryTipo != null && queryTipo != "")
                {
                    leitor = con.RetornarAdaptador(queryTipo).SelectCommand.ExecuteReader();

                    if (leitor.Read())
                    {
                        entidade.Tipo = new TipoEntidade()
                        {
                            Codigo = int.Parse(leitor[0].ToString()),
                            Nome = leitor[1].ToString()
                        };
                    }
                    Util.FinalizarDataReader(leitor);
                }
            }
            return entidade;
        }
    }
}

