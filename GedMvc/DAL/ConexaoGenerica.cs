using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DAL
{
    public class ConexaoGenerica : IDisposable
    {
        private string _StringConnection = string.Empty;
        private DbConnection Conexao;
        private DbDataAdapter Adaptador;
        private DbTransaction Transaction;

        /// <summary>
        /// Verifica o estado da conexão se esta aberto ou não
        /// </summary>
        public bool ConnectionOpened;

        /// <summary>
        /// Informa se esta trabalhando em regime de transaction
        /// </summary>
        public bool IsTransaction;

        private bool disposed = false;

        /// <summary>
        ///Adaptador(DataAdapter) de providers generico, é obrigatório já passar a query sql e a string de
        ///conexão ou resultará em exception 
        /// </summary>
        /// <param name="adaptador">DataAdapter abstrato podendo ser SqlDataAdapter, OleDbDataAdapter, MySqlDataAdapter, OleDbDataAdapter, etc...</param>
        /// <param name="isTransaction">Inicia o objeto usando ou não transaction na conexão e nos objetos de comando</param>
        public ConexaoGenerica(DbDataAdapter adaptador, bool isTransaction)
        {
            //if (adaptador.SelectCommand == null)
            //{
            //    throw new Exception("Contrutor do Adaptador precisa ser iniciado com a query sql e a conexao sql");
            //}

            //if (string.IsNullOrEmpty(adaptador.SelectCommand.CommandText))
            //{
            //    throw new Exception("Query sql inválida!");
            //}

            if (string.IsNullOrEmpty(adaptador.SelectCommand.Connection.ConnectionString))
            {
                throw new Exception("String de conexão sql inválida!");
            }

            this.IsTransaction = isTransaction;

            Conexao = adaptador.SelectCommand.Connection;
            Adaptador = adaptador;
        }

        #region IConnection Members

        /// <summary>
        /// Responsável pelas operações de CRUD. 
        /// Retorna um objeto do tipo DataAdapter com a query padrão definida na instanciação do objeto ConexaoGenerica
        /// </summary>
        /// <returns>Objetos do tipo SqlDataAdapter, OleDbDataAdapter, etc...</returns>
        public DbDataAdapter RetornarAdaptador()
        {
            if (Conexao.State == System.Data.ConnectionState.Closed)
            {
                Conexao.Open();
                this.ConnectionOpened = true;

                if (IsTransaction)
                {
                    Transaction = Conexao.BeginTransaction();
                    this.Adaptador.SelectCommand.Transaction = Transaction;
                }
            }
            return Adaptador;
        }

        /// <summary>
        /// Responsável pelas operações de CRUD. 
        /// Sobrecarga do método RetornarAdaptador() passando como argumento query sql.        
        /// </summary>
        /// <param name="query">Query sql disparada contra o banco</param>
        /// <returns>Objetos do tipo SqlDataAdapter, OleDbDataAdapter, etc...</returns>
        public System.Data.Common.DbDataAdapter RetornarAdaptador(string query)
        {
            Adaptador.SelectCommand.Connection = Conexao;
            Adaptador.SelectCommand.CommandText = query;

            if (Conexao.State == System.Data.ConnectionState.Closed)
            {
                Conexao.Open();
                this.ConnectionOpened = true;

                if (IsTransaction)
                {
                    Transaction = Conexao.BeginTransaction();
                    this.Adaptador.SelectCommand.Transaction = Transaction;
                }
            }

            if (Adaptador.SelectCommand == null)
            {
                throw new Exception("Ao instanciar o adaptador é obrigatório definir a query sql");
            }

            return this.Adaptador;
        }

        #endregion


        private static ConexaoGenerica conexaoGenerica;
        public static ConexaoGenerica SingleTonObject(DbDataAdapter ad,bool isTransaction)
        {
            if (conexaoGenerica == null)
            {
                conexaoGenerica = new ConexaoGenerica(ad, isTransaction);
            }

            return conexaoGenerica;
        }

        #region IDisposable Members

        /// <summary>
        /// Fecha a conexão e realiza dispose, caso haja transaction commita, realiza dispose com os objetos Command e DataAdapter e chama o GC.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Fecha a conexão e realiza dispose, caso haja transaction commita, realiza dispose com os objetos Command e DataAdapter
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.Conexao.State == System.Data.ConnectionState.Open)
                    {
                        if (IsTransaction)
                        {
                            Transaction.Commit();
                        }

                        this.Conexao.Close();
                        this.Conexao.Dispose();
                        this.ConnectionOpened = false;
                    }

                    this.Adaptador.SelectCommand.Dispose();
                    this.Adaptador.Dispose();
                }
                disposed = true;
            }
        }

        #endregion

        /// <summary>
        /// Faz o rollback quando a conexão esta com transaction
        /// </summary>
        public void DoRollBack()
        {
            if (IsTransaction)
            {
                this.Transaction.Rollback();
                IsTransaction = false;
            }
        }

        ~ConexaoGenerica()
        {
            Dispose(false);
        }

    }

}
