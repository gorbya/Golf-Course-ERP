using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LinksLabGolfSystem.SQL
{
    public class SQLDataService
    {
        private string _connectionString = "Server=localhost\\SQLEXPRESS;Database=LinksLabGolfSystem;Trusted_Connection=True;";

        public SQLDataService()
        {
            _connectionString = "Server=localhost\\SQLEXPRESS;Database=LinksLabGolfSystem;Trusted_Connection=True;";
        }

        // Method to execute a SELECT query and return results
        public DataTable ExecuteSelectQuery(string query)
        {
            DataTable resultTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                connection.Open();
                dataAdapter.Fill(resultTable);
            }

            return resultTable;
        }

        // Method to execute an INSERT, UPDATE, or DELETE query
        public int ExecuteNonQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        // Method to execute a query with parameters (e.g., for INSERT, UPDATE)
        public int ExecuteNonQueryWithParams(string query, List<SqlParameter> parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddRange(parameters.ToArray());
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        // Method to get a single value (e.g., from a SELECT query with aggregate functions)
        public object ExecuteScalarQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                return command.ExecuteScalar();
            }
        }
    }
}