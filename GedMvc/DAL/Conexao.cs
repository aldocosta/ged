using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Configuration;

namespace DAL
{
    public class Conexao
    {
        private static Conexao instance;
        public string StringConexao { get; set; }

        public Conexao()
        {   
            StringConexao = ConfigurationManager.ConnectionStrings["banco"].ConnectionString;
        }

        public static Conexao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Conexao();
                }
                return instance;
            }
        }

        /// <summary>
        /// REALIZA INSERT, UPDATE E DELETE
        /// </summary>
        /// <param name="ad"></param>
        /// <returns></returns>
        public static int RealizarIUD(DbDataAdapter ad)
        {
            int ret = 0;
            using (DAL.ConexaoGenerica con = new DAL.ConexaoGenerica(ad, false))
            {
                ret = con.RetornarAdaptador().SelectCommand.ExecuteNonQuery();
            }
            return ret;
        }

        /// <summary>
        /// REALIZA INSERT, UPDATE E DELETE
        /// </summary>
        /// <param name="ad"></param>
        /// <returns></returns>
        public static int RealizarIUD_LastIdentifier(DbDataAdapter ad)
        {
            int ret = 0;
            using (DAL.ConexaoGenerica con = new DAL.ConexaoGenerica(ad, false))
            {
                ret = Convert.ToInt32(con.RetornarAdaptador().SelectCommand.ExecuteScalar());
            }
            return ret;
        }

        public static DbDataReader RetornarDataReader(ConexaoGenerica con, string query)
        {
            return con.RetornarAdaptador(query).SelectCommand.ExecuteReader();
        }
    }
}
