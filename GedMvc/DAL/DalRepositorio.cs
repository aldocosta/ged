using DEEntities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DAL
{
    public class DalRepositorio : IIUD<Repositorio>
    {
        public int Save(Repositorio t)
        {
            string queryinsert = "insert into TbRepositorio(cdProcesso,nm_arquivo,nm_extensao,nr_tamanho,nm_caminho_disco,cdentidade)" +
                " values(" + t.CdProcesso + ",'" + t.NomeArquivo + "','" + t.Extensao + "','" + t.Tamanho + "','" + t.Caminho + "'," + t.CdEntidade + ");select LAST_INSERT_ID();";
            return DAL.Conexao.RealizarIUD_LastIdentifier(new MySqlDataAdapter(queryinsert, Conexao.Instance.StringConexao));
        }

        public bool Delete(Repositorio t)
        {
            string query = "delete from TbRepositorio where cdRepositorio=" + t.CdRepositorio + ";";
            return DAL.Conexao.RealizarIUD(new MySqlDataAdapter(query, Conexao.Instance.StringConexao)) > 0;
        }

        public bool Update(Repositorio t)
        {
            throw new NotImplementedException();
        }

        public IList<Repositorio> RetornarLista()
        {
            throw new NotImplementedException();
        }

        public IList<Repositorio> RetornarLista(Repositorio t)
        {
            throw new NotImplementedException();
        }

        public IList<Repositorio> RetornarListaPorDeptoUsuario(string iddepto, string chave)
        {
            IList<Repositorio> lista = new List<Repositorio>();
            string query = "pesquisarRepositorioUsuario";
            using (ConexaoGenerica con = new ConexaoGenerica(new MySqlDataAdapter(query, Conexao.Instance.StringConexao), false))
            {
                MySqlCommand mcom = con.RetornarAdaptador().SelectCommand as MySqlCommand;
                mcom.Parameters.AddWithValue("cdEntidade", iddepto);
                mcom.Parameters.AddWithValue("pesq", chave);
                mcom.CommandType = System.Data.CommandType.StoredProcedure;
                DbDataReader leitor = mcom.ExecuteReader();

                while (leitor.Read())
                {
                    Repositorio rep = new Repositorio();
                    rep.CdRepositorio = int.Parse(leitor[0].ToString());
                    rep.CdProcesso = int.Parse(leitor[1].ToString());
                    rep.NomeArquivo = leitor[2].ToString();
                    rep.Extensao = leitor[3].ToString();
                    rep.Tamanho = Convert.ToInt64(leitor[4]);
                    rep.Caminho = leitor[5].ToString();
                    rep.CdEntidade = int.Parse(leitor[11].ToString());
                    rep.CdPasta = int.Parse(leitor[12].ToString());
                    rep.NomeFisicoArquivo = "Pasta/Processo: (" + leitor[9].ToString() + " ) - Depto: (" + leitor[10].ToString() + ")";
                    rep.Owner = new DALEntidade().RetornarEntidade(rep.CdEntidade);
                    lista.Add(rep);
                }
                Utilities.Util.FinalizarDataReader(leitor);
            }
            return lista;
        }

        public Repositorio RetornarEntidade(int id)
        {
            throw new NotImplementedException();
        }
    }
}
