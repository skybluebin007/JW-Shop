namespace SkyCES.EntLib
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;

    public sealed class AccessHelper
    {
        private string connectionString = string.Empty;

        public DataTable ExecuteDataTable(string commandText)
        {
            return this.ExecuteDataTable(commandText, null);
        }

        public DataTable ExecuteDataTable(string commandText, OleDbParameter[] pt)
        {
            DataTable dataTable = new DataTable();
            using (AccessPoolManager.AccessPool pool = AccessPoolManager.Instance(this.connectionString))
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(commandText, pool.Connection))
                {
                    if (pt != null)
                    {
                        adapter.SelectCommand.Parameters.AddRange(pt);
                    }
                    adapter.Fill(dataTable);
                }
            }
            return dataTable;
        }

        public void ExecuteNonQuery(string commandText)
        {
            this.ExecuteNonQuery(commandText, null);
        }

        public void ExecuteNonQuery(List<string> commandTexts)
        {
            using (AccessPoolManager.AccessPool pool = AccessPoolManager.Instance(this.connectionString))
            {
                using (OleDbTransaction transaction = pool.Connection.BeginTransaction())
                {
                    using (OleDbCommand command = new OleDbCommand())
                    {
                        try
                        {
                            command.Connection = pool.Connection;
                            command.Transaction = transaction;
                            foreach (string str in commandTexts)
                            {
                                command.CommandText = str;
                                command.ExecuteNonQuery();
                            }
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
        }

        public void ExecuteNonQuery(string commandText, OleDbParameter[] pt)
        {
            using (AccessPoolManager.AccessPool pool = AccessPoolManager.Instance(this.connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(commandText, pool.Connection))
                {
                    if (pt != null)
                    {
                        command.Parameters.AddRange(pt);
                    }
                    command.ExecuteNonQuery();
                }
            }
        }

        public OleDbDataReader ExecuteReader(string commandText)
        {
            return this.ExecuteReader(commandText, null);
        }

        public OleDbDataReader ExecuteReader(string commandText, OleDbParameter[] pt)
        {
            AccessPoolManager.AccessPool pool = AccessPoolManager.Instance(this.connectionString);
            using (OleDbCommand command = new OleDbCommand(commandText, pool.Connection))
            {
                if (pt != null)
                {
                    command.Parameters.AddRange(pt);
                }
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        public object ExecuteScalar(string commandText)
        {
            return this.ExecuteScalar(commandText, null);
        }

        public object ExecuteScalar(string commandText, OleDbParameter[] pt)
        {
            object obj2;
            using (AccessPoolManager.AccessPool pool = AccessPoolManager.Instance(this.connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(commandText, pool.Connection))
                {
                    if (pt != null)
                    {
                        command.Parameters.AddRange(pt);
                    }
                    obj2 = command.ExecuteScalar();
                }
            }
            return obj2;
        }

        public string ConnectionString
        {
            get
            {
                return this.connectionString;
            }
            set
            {
                this.connectionString = value;
            }
        }
    }
}

