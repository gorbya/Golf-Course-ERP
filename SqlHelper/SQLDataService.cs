using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SqlHelper
{
    public class SQLDataService
    {
        private string _connectionString = "";

        public SQLDataService(string connection) {
            _connectionString = connection;
        }

        public DataTable ExecuteSelectQuery(string query) {
            DataTable resultTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                connection.Open();
                dataAdapter.Fill(resultTable);
            }

            return resultTable;
        }

        public int ExecuteNonQuery(string query) {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public int ExecuteNonQueryWithParams(string query, List<SqlParameter> parameters) {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddRange(parameters.ToArray());
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public object ExecuteScalarQuery(string query) {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                return command.ExecuteScalar();
            }
        }
    }
}