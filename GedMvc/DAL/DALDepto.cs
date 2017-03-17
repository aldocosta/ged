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
    public class DALDepto : IIUD<Depto>
    {
        public int Save(Depto t)
        {
            string query = "insert into TbDepto(nmDepto,DtCriacao,cdentidade) values('" + t.NomeDepto + "',current_date," + t.CdEntidade + ");select LAST_INSERT_ID() ;";
            int ret = Conexao.RealizarIUD_LastIdentifier(new MySqlDataAdapter(query, Conexao.Instance.StringConexao));
            return ret; ;
        }

        public bool Delete(Depto t)
        {
            bool ret = false;
            string query = "delete from TbDepto where cdDepto=" + t.Id;
            ret = Conexao.RealizarIUD(new MySqlDataAdapter(query, Conexao.Instance.StringConexao)) > 0;
            return ret;
        }

        public bool Update(Depto t)
        {
            string query = "update TbDepto set nmDepto ='" + t.NomeDepto + "' where cdDepto =" + t.Id;
            int ret = Conexao.RealizarIUD(new MySqlDataAdapter(query, Conexao.Instance.StringConexao));
            return ret > 0;
        }

        public IList<Depto> RetornarLista()
        {
            IList<Depto> lista = new List<Depto>();            
            using (ConexaoGenerica con = new ConexaoGenerica(new MySqlDataAdapter("RetornarDeptos", Conexao.Instance.StringConexao), false))
            {
                MySqlCommand mcom = con.RetornarAdaptador().SelectCommand as MySqlCommand;
                mcom.CommandType = System.Data.CommandType.StoredProcedure;
                DbDataReader leitor = mcom.ExecuteReader();
                while (leitor.Read())
                {
                    lista.Add(new Depto()
                    {
                        Id = int.Parse(leitor[0].ToString()),
                        NomeDepto = leitor[1].ToString(),
                        DataCriacao = Convert.ToDateTime(leitor[2].ToString()),
                        CdEntidade = Convert.ToInt32(leitor[3]),
                        OwnerName = leitor[4].ToString()
                    });
                }
                Util.FinalizarDataReader(leitor);
                //RetornarDeptos
                //DbDataReader leitor = con.RetornarAdaptador().SelectCommand.ExecuteReader();
                //while (leitor.Read())
                //{
                //    lista.Add(new Depto()
                //    {
                //        Id = int.Parse(leitor[0].ToString()),
                //        NomeDepto = leitor[1].ToString(),
                //        DataCriacao = Convert.ToDateTime(leitor[2].ToString()),
                //        CdEntidade = Convert.ToInt32(leitor[3]),
                //        Owner = new DALEntidade().RetornarEntidade(Convert.ToInt32(leitor[3]))
                //    });
                //}
                //Util.FinalizarDataReader(leitor);
            }
            return lista;
        }

        public IList<Depto> RetornarLista(Depto t)
        {
            IList<Depto> lista = new List<Depto>();
            string query = "select * from TbDepto where cdDepto =" + t.Id;
            using (ConexaoGenerica con = new ConexaoGenerica(new MySqlDataAdapter(query, Conexao.Instance.StringConexao), false))
            {
                DbDataReader leitor = con.RetornarAdaptador().SelectCommand.ExecuteReader();
                while (leitor.Read())
                {
                    lista.Add(new Depto()
                    {
                        Id = int.Parse(leitor[0].ToString()),
                        NomeDepto = leitor[1].ToString()
                    });
                }
                Util.FinalizarDataReader(leitor);
            }
            return lista;
        }

        public IList<RetornarProcessosDeptoView> RetornarProcessosDepto(Depto d)
        {
            IList<RetornarProcessosDeptoView> lista = new List<RetornarProcessosDeptoView>();

            using (ConexaoGenerica con = new ConexaoGenerica(new MySqlDataAdapter("RetornarProcessosDepto",
                Conexao.Instance.StringConexao), false))
            {
                MySqlCommand com = con.RetornarAdaptador().SelectCommand as MySqlCommand;
                com.Parameters.AddWithValue("cddepto", d.Id);
                com.CommandType = System.Data.CommandType.StoredProcedure;
                DbDataReader leitor = com.ExecuteReader();

                while (leitor.Read())
                {
                    lista.Add(new RetornarProcessosDeptoView()
                    {
                        NomeProcesso = leitor[0].ToString(),
                        DescricaoProcesso = leitor[1].ToString(),
                        Departamento = leitor[2].ToString(),
                        DataCriacao = Convert.ToDateTime(leitor[3].ToString()),
                        Status = leitor[4].ToString()
                    });
                }

                Util.FinalizarDataReader(leitor);
            }
            return lista;
        }


        public Depto RetornarEntidade(int id)
        {
            Depto depto = new Depto();
            string query = "select * from TbDepto where cdDepto=" + id;
            using (ConexaoGenerica con = new ConexaoGenerica(new MySqlDataAdapter(query, Conexao.Instance.StringConexao), false))
            {
                DbDataReader leitor = con.RetornarAdaptador().SelectCommand.ExecuteReader();

                while (leitor.Read())
                {
                    depto.Id = int.Parse(leitor[0].ToString());
                    depto.NomeDepto = leitor[1].ToString();
                    depto.DataCriacao = Convert.ToDateTime(leitor[2].ToString());
                }
                Util.FinalizarDataReader(leitor);
            }
            return depto;
        }
    }
}
