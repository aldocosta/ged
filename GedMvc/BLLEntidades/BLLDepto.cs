using DAL;
using DEEntities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace BLLEntidades
{
    public class BLLDepto
    {
        static DALDepto daldepto = new DALDepto();

        public static bool Atualizar(string id, string nmDepto)
        {
            return daldepto.Update(new Depto()
            {
                NomeDepto = nmDepto,
                Id = int.Parse(id)
            });
        }

        public static int GravarDepto(Depto depto)
        {

            int ret = 0;
            string queryInsert = "insert into tbDepto(nmDepto,dtCriacao)";
            queryInsert += "values('" + depto.NomeDepto + "',current_date); select LAST_INSERT_ID() ; ";

            ret = daldepto.Save(depto);

            //ret = Conexao.RealizarIUD_LastIdentifier(new MySqlDataAdapter(queryInsert, Conexao.Instance.StringConexao));
            depto.Id = ret;
            return ret;
        }
        public static bool DeletarDepto(int codigo)
        {
            bool ret = false; ;
            string query = "delete from tbDepto where cdDepto = " + codigo;
            ret = daldepto.Delete(new Depto()
            {
                Id = codigo
            });            

            return ret;
        }

        public static IList<Depto> RetornarDeptos()
        {
            IList<Depto> ret = daldepto.RetornarLista();
            return ret;
        }
        public static List<Depto> RetornarDeptoEntidade(Entidade e, ConexaoGenerica con)
        {
            List<Depto> lista = new List<Depto>(100);
            string query = "SELECT d.* from tbEntidadeDepto ed join TbEntidade e on ed.cdEntidade = e.cdEntidade join tbDepto d on ed.cdDepto = d.cdDepto;";
            query += "where e.cdEntidade =" + e.Codigo;
            DbDataReader leitor = con.RetornarAdaptador(query).SelectCommand.ExecuteReader();

            while (leitor.Read())
            {
                lista.Add(new Depto()
                {
                    Id = int.Parse(leitor[0].ToString()),
                    NomeDepto = leitor[1].ToString()
                });
            }

            return lista;
        }

        public static IList<RetornarProcessosDeptoView> RetornarProcessosDepto(Depto d)
        {
            return daldepto.RetornarProcessosDepto(d);
        }
    }
}
