using DEEntities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Utilities;

namespace DAL
{
    public class DALEntidadeDepto : IIUD<EntidadeDepto>
    {
        public int Save(EntidadeDepto t)
        {
            string query = "insert into TbEntidadeDepto(cdEntidade,cdDepto) values(" + t.cdEntidade + "," + t.cdDepto + ");select LAST_INSERT_ID() ";
            int ret = Conexao.RealizarIUD_LastIdentifier(new MySqlDataAdapter(query, Conexao.Instance.StringConexao));
            return ret;
        }

        public bool Delete(EntidadeDepto t)
        {
            string query = "delete from TbEntidadeDepto where cdEntidade=" + t.cdEntidade + " and cdDepto=" + t.cdDepto;
            return Conexao.RealizarIUD(new MySqlDataAdapter(query, Conexao.Instance.StringConexao)) > 0;
        }

        public bool Update(EntidadeDepto t)
        {
            string query = "update TbEntidadeDepto set cdEntidade=" + t.cdEntidade + ",cdDepto = " + t.cdDepto;

            return Conexao.RealizarIUD(new MySqlDataAdapter(query, Conexao.Instance.StringConexao)) > 0;
        }

        public IList<EntidadeDepto> RetornarLista()
        {
            IList<EntidadeDepto> ret = new List<EntidadeDepto>();
            string query = "SELECT ed.cdEntidade,ed.cdDepto,e.nmEntidade,d.nmDepto from TbEntidadeDepto ed " +
            " join TbEntidade e on ed.cdEntidade = e.cdEntidade " +
            " join tbDepto d on ed.cdDepto = d.cdDepto;";

            using (ConexaoGenerica con = new ConexaoGenerica(new MySqlDataAdapter(query, Conexao.Instance.StringConexao), false))
            {
                DbDataReader leitor = con.RetornarAdaptador().SelectCommand.ExecuteReader();

                while (leitor.Read())
                {
                    ret.Add(new EntidadeDepto()
                    {
                        cdEntidade = int.Parse(leitor[0].ToString()),
                        cdDepto = int.Parse(leitor[1].ToString()),
                        NmEntidade = leitor[2].ToString(),
                        NmDepto = leitor[3].ToString()
                    });
                }
                Util.FinalizarDataReader(leitor);
            }
            return ret;
        }

        public IList<EntidadeDepto> RetornarLista(EntidadeDepto t)
        {
            IList<EntidadeDepto> ret = new List<EntidadeDepto>();

            using (ConexaoGenerica con = new ConexaoGenerica(new MySqlDataAdapter("RetornarDeptosEntidade", Conexao.Instance.StringConexao), false))
            {
                MySqlCommand comando = con.RetornarAdaptador().SelectCommand as MySqlCommand;
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("cdEntidade", t.cdEntidade);
                DbDataReader leitor = comando.ExecuteReader();                

                while (leitor.Read())
                {
                    ret.Add(new EntidadeDepto()
                    {
                        cdEntidade = int.Parse(leitor[0].ToString()),
                        cdDepto = int.Parse(leitor[1].ToString()),
                        NmEntidade = leitor[2].ToString(),
                        NmDepto = leitor[3].ToString()
                    });
                }
                Util.FinalizarDataReader(leitor);
            }
            return ret;
        }


        public EntidadeDepto RetornarEntidade(int id)
        {
            throw new NotImplementedException();
        }
    }
}
