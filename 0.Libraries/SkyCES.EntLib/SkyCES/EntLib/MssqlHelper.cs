namespace SkyCES.EntLib
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public sealed class MssqlHelper
    {
        private string connectionString = string.Empty;

        public DataTable ExecuteDataTable(string storedProcName)
        {
            return this.ExecuteDataTable(storedProcName, null);
        }

        public DataTable ExecuteDataTable(string storedProcName, SqlParameter[] pt)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                DataTable dataTable = new DataTable();
                using (SqlDataAdapter adapter = new SqlDataAdapter(storedProcName, connection))
                {
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    if (pt != null)
                    {
                        adapter.SelectCommand.Parameters.AddRange(pt);
                    }
                    adapter.Fill(dataTable);
                }
                return dataTable;
            }
        }

        public void ExecuteNonQuery(string storedProcName)
        {
            this.ExecuteNonQuery(storedProcName, null);
        }

        public void ExecuteNonQuery(string storedProcName, SqlParameter[] pt)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    this.PrepareCommand(connection, command, storedProcName, pt);
                    command.ExecuteNonQuery();
                }
            }
        }

        public SqlDataReader ExecuteReader(string storedProcName)
        {
            return this.ExecuteReader(storedProcName, null);
        }

        public SqlDataReader ExecuteReader(string storedProcName, SqlParameter[] pt)
        {
            SqlConnection connection = new SqlConnection(this.connectionString);
            using (SqlCommand command = new SqlCommand(storedProcName, connection))
            {
                this.PrepareCommand(connection, command, storedProcName, pt);
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        public object ExecuteScalar(string storedProcName)
        {
            return this.ExecuteScalar(storedProcName, null);
        }

        public object ExecuteScalar(string storedProcName, SqlParameter[] pt)
        {
            object obj2;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    this.PrepareCommand(connection, command, storedProcName, pt);
                    obj2 = command.ExecuteScalar();
                }
            }
            return obj2;
        }

        private void PrepareCommand(SqlConnection conn, SqlCommand cmd, string storedProcName, SqlParameter[] pt)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storedProcName;
            if (pt != null)
            {
                cmd.Parameters.AddRange(pt);
            }
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

