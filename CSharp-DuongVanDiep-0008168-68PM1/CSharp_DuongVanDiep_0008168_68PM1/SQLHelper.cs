using System;
using System.Data;
using System.Data.SqlClient;

namespace CSharp_DuongVanDiep_0008168_68PM1
{
    internal class SQLHelper
    {
        private string connectionString = @"Data Source=WINDOWS-PC;Initial Catalog=QuanLySinhVienDB;Integrated Security=True;";

        public DataTable ExecuteQuery(string query)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }

        public DataTable ExecuteQuery(string query, SqlParameter[] parameters)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                        command.Parameters.AddRange(parameters);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }

        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                        command.Parameters.AddRange(parameters);
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            return rowsAffected;
        }

        public object ExecuteScalar(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    return command.ExecuteScalar();
                }
            }
        }

        public object ExecuteScalar(string query, SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                        command.Parameters.AddRange(parameters);
                    return command.ExecuteScalar();
                }
            }
        }
    }
}