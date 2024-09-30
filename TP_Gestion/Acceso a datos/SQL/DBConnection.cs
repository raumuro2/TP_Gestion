using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using TP_Gestion.Models;

namespace TP_Gestion.Acceso_a_datos
{
    public class DBConnection
    {
        private string connectionString;

        public DBConnection(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public bool Insert(string tableName, Dictionary<string, object> data, out long id)
        {
            id = 0;

            try
            {
                string columns = string.Join(", ", data.Keys);
                string parameters = string.Join(", ", data.Keys.Select(key => $"@{key}"));
                string sqlQuery = $@"
            INSERT INTO {tableName} ({columns}) 
            VALUES ({parameters});
            SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    id = connection.QuerySingle<long>(sqlQuery, data);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Update(string tableName, Dictionary<string, object> data, string whereClause, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                string setClause = string.Join(", ", data.Keys.Select(key => $"{key} = @{key}"));
                string sqlQuery = $"UPDATE {tableName} SET {setClause} WHERE {whereClause}";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    connection.Execute(sqlQuery, data);
                }
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public bool UpdateMultiplePersonas(List<Persona> personas, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string sqlQuery = SQLQueries.UpdateEnAlta;

                            connection.Execute(sqlQuery, personas, transaction);
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public bool Delete(string tableName, string columnName, object value, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                string sqlQuery = $"DELETE FROM {tableName} WHERE {columnName} = @{columnName}";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var parameters = new DynamicParameters();
                    parameters.Add($"@{columnName}", value);

                    connection.Execute(sqlQuery, parameters);
                }

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
        //Método genérico para consultas de una tabla

        public IEnumerable<T> ExecuteQuery<T>(string query, object parameters = null)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    return connection.Query<T>(query, parameters);
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        //Método genérico para consultas de dos tablas
        public IEnumerable<T1> ExecuteQuery<T1, T2>(string query, Func<T1, T2, T1> map, 
            object parameters = null, string splitOn = "Id")
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    return connection.Query(query, map, parameters, splitOn: splitOn);
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
                throw; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw; 
            }
        }
    }
}
