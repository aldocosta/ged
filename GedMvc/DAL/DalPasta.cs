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
    public class DalPasta : IIUD<Pasta>
    {
        public int Save(Pasta t)
        {
            string campos = "nmpasta,dtcriacao,dspasta,cddepto,cdentidade";
            string values = "'" + t.NmPasta + "',now(),'" + t.DsPasta + "'," + t.CodigoDepto + "," + t.CdEntidade;
            if (t.CodigoPai != 0)
            {
                campos += ",cdpastapai";
                values += "," + t.CodigoPai + "";
            }

            return DAL.Conexao.RealizarIUD_LastIdentifier(new MySqlDataAdapter("insert into TbPasta(" + campos + ")" +
            "values(" + values + ");select LAST_INSERT_ID();", DAL.Conexao.Instance.StringConexao));
        }

        public bool Delete(Pasta t)
        {
            int ret = DAL.Conexao.RealizarIUD(new MySqlDataAdapter("delete from TbPasta where cdpasta=" + t.Codigo + "", DAL.Conexao.Instance.StringConexao));
            return ret != 0;
        }

        public bool Update(Pasta t)
        {
            int ret = DAL.Conexao.RealizarIUD(new MySqlDataAdapter("update TbPasta set nmpasta='" + t.NmPasta
                + "', dspasta='" + t.DsPasta + "'  where cdpasta=" + t.Codigo + "", DAL.Conexao.Instance.StringConexao));
            return ret != 0;
        }

        public IList<Pasta> RetornarLista()
        {
            List<Pasta> lp = new List<Pasta>();
            string query = "select * from TbPasta;";
            using (ConexaoGenerica con = new ConexaoGenerica(new MySqlDataAdapter(query, Conexao.Instance.StringConexao), false))
            {
                DbDataReader leitor = con.RetornarAdaptador().SelectCommand.ExecuteReader();
                while (leitor.Read())
                {
                    lp.Add(new Pasta()
                    {
                        Codigo = int.Parse(leitor[0].ToString()),
                        NmPasta = leitor[1].ToString(),
                        DtCriacao = Convert.ToDateTime(leitor[2].ToString()),
                        DsPasta = leitor[3].ToString(),
                        CodigoDepto = int.Parse(leitor[4].ToString()),
                        CodigoPai = leitor[5].ToString() != "" ? int.Parse(leitor[5].ToString()) : 0,
                        CdEntidade = int.Parse(leitor[6].ToString()),
                        Owner = new DALEntidade().RetornarEntidade(int.Parse(leitor[6].ToString()))
                    });
                }
                Util.FinalizarDataReader(leitor);
            }
            return lp;
        }

        public IList<Pasta> RetornarLista(Pasta t)
        {
            List<Pasta> lp = new List<Pasta>();
            string query = "select * from TbPasta where cdpasta=" + t.Codigo + ";";
            using (ConexaoGenerica con = new ConexaoGenerica(new MySqlDataAdapter(query, Conexao.Instance.StringConexao), false))
            {
                DbDataReader leitor = con.RetornarAdaptador().SelectCommand.ExecuteReader();
                while (leitor.Read())
                {
                    lp.Add(new Pasta()
                    {
                        Codigo = int.Parse(leitor[0].ToString()),
                        NmPasta = leitor[1].ToString(),
                        DtCriacao = Convert.ToDateTime(leitor[2].ToString()),
                        DsPasta = leitor[3].ToString(),
                        CodigoDepto = int.Parse(leitor[4].ToString()),
                        CdEntidade = int.Parse(leitor[6].ToString()),
                        Owner = new DALEntidade().RetornarEntidade(int.Parse(leitor[6].ToString()))
                    });
                }
                Util.FinalizarDataReader(leitor);
            }
            return lp;
        }

        public Pasta RetornarEntidade(int id)
        {
            Pasta p = null;
            string query = "select * from TbPasta where cdpasta=" + id;
            using (ConexaoGenerica con = new ConexaoGenerica(new MySql.Data.MySqlClient.MySqlDataAdapter(query, Conexao.Instance.StringConexao), false))
            {
                DbDataReader leitor = con.RetornarAdaptador().SelectCommand.ExecuteReader();

                while (leitor.Read())
                {
                    p = new Pasta();
                    p.Codigo = int.Parse(leitor[0].ToString());
                    p.NmPasta = leitor[1].ToString();
                    p.DtCriacao = Convert.ToDateTime(leitor[2].ToString());
                    p.DsPasta = leitor[3].ToString();
                    p.CodigoDepto = int.Parse(leitor[4].ToString());
                    p.CdEntidade = int.Parse(leitor[6].ToString());

                    int temp = 0;
                    int.TryParse(leitor[5].ToString(), out temp);
                    p.CodigoPai = temp;

                    if (p.CodigoPai != 0)
                        p.PastaPai = this.RetornarEntidade(p.CodigoPai);


                    p.Owner = new DALEntidade().RetornarEntidade(int.Parse(leitor[6].ToString()));

                }
            }
            return p;
        }


    }
}

