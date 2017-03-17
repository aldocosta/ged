using DEEntities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DAL
{
    public class DALProcesso : IIUD<Processos>
    {
        public int Save(Processos t)
        {
            int ret = 0;

            //aonde IdPai é o id da pasta
            string campos = "nmProcesso,dsProcesso,cdpasta,cdentidade";
            string valores = "'" + t.NomeProcesso + "','" + t.Descricao + "'," + t.IdPai + "," + t.CdEntidade + "";

            string queryInsert = "insert into TbProcesso(" + campos + ") values(" + valores + ");select LAST_INSERT_ID();";


            ret = Conexao.RealizarIUD_LastIdentifier(new MySqlDataAdapter(queryInsert, Conexao.Instance.StringConexao));
            return ret;
        }

        public bool Delete(Processos t)
        {
            int ret = DAL.Conexao.RealizarIUD(new MySqlDataAdapter("delete from TbProcesso where cdProcesso=" + t.Id + "", DAL.Conexao.Instance.StringConexao));
            return ret != 0;
        }

        public bool Update(Processos t)
        {
            int ret = DAL.Conexao.RealizarIUD(new MySqlDataAdapter("update TbProcesso set nmProcesso='" + t.NomeProcesso
                + "', dsProcesso='" + t.Descricao + "'  where cdProcesso=" + t.Id + " and cdpasta=" + t.IdPai + "", DAL.Conexao.Instance.StringConexao));
            return ret != 0;
        }

        public IList<Processos> RetornarLista()
        {
            List<Processos> lp = new List<Processos>();
            string query = "select * from TbProcesso;";
            using (ConexaoGenerica con = new ConexaoGenerica(new MySqlDataAdapter(query, Conexao.Instance.StringConexao), false))
            {
                DbDataReader leitor = con.RetornarAdaptador().SelectCommand.ExecuteReader();
                while (leitor.Read())
                {
                    lp.Add(new Processos()
                    {
                        Id = int.Parse(leitor[0].ToString()),
                        NomeProcesso = leitor[1].ToString(),
                        Descricao = leitor[2].ToString(),
                        IdPai = int.Parse(leitor[5].ToString()),
                        CdEntidade = int.Parse(leitor[6].ToString()),
                        Owner = new DALEntidade().RetornarEntidade(int.Parse(leitor[6].ToString()))
                    });
                }
                Utilities.Util.FinalizarDataReader(leitor);
            }

            return lp;
        }

        public IList<Processos> RetornarLista(Processos t)
        {
            throw new NotImplementedException();
        }


        public Processos RetornarEntidade(int id)
        {
            DALEntidade entidade = new DALEntidade();
            Processos procs = null;
            string query = "select pro.cdProcesso,pro.nmProcesso,pro.dsProcesso,pro.cdTipoProcesso,pro.cdProcessoPai, " +
            "pro.cdPasta,pasta.nmpasta " +
            "from TbProcesso pro " +
            "join TbPasta pasta on pro.cdpasta = pasta.cdpasta where cdProcesso = " + id;
            //string query = "select * from TbProcesso where cdProcesso =" + id;
            using (ConexaoGenerica con = new ConexaoGenerica(new MySqlDataAdapter(query, Conexao.Instance.StringConexao), false))
            {
                DbDataReader leitor = con.RetornarAdaptador().SelectCommand.ExecuteReader();
                if (leitor.Read())
                {
                    procs = new Processos();
                    procs.Id = int.Parse(leitor[0].ToString());
                    procs.NomeProcesso = leitor[1].ToString();
                    procs.Descricao = leitor[2].ToString();
                    procs.IdPai = int.Parse(leitor[5].ToString());//pro.cdPasta
                    procs.PastaPai = leitor[6].ToString();
                }
                Utilities.Util.FinalizarDataReader(leitor);


                string queryarquivos = "select * from TbRepositorio where cdProcesso=" + id;
                leitor = con.RetornarAdaptador(queryarquivos).SelectCommand.ExecuteReader();

                List<Repositorio> listarepo = new List<Repositorio>();

                while (leitor.Read())
                {
                    Repositorio repo = new Repositorio();
                    repo.CdRepositorio = int.Parse(leitor[0].ToString());
                    repo.CdProcesso = int.Parse(leitor[1].ToString());
                    repo.NomeArquivo = leitor[2].ToString();
                    repo.Extensao = leitor[3].ToString();
                    repo.Tamanho = Convert.ToInt64(leitor[4]);
                    repo.Caminho = leitor[5].ToString();
                    repo.CdEntidade = Convert.ToInt32(leitor[6]);
                    repo.Owner = entidade.RetornarEntidade(repo.CdEntidade);

                    listarepo.Add(repo);

                }
                Utilities.Util.FinalizarDataReader(leitor);



                if (procs != null)
                {
                    procs.Arquivos = listarepo;
                }
            }

            return procs;
        }
    }
}
