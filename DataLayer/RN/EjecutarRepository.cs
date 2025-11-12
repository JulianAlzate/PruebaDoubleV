using Dapper;
using DataLayer.Interface;
using Microsoft.Data.SqlClient;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.RN
{
    public class EjecutarRepository : IEjecutarRepository
    {

        public async Task<T> ExecuteScalarAsync<T>(string functionName, string connectionString, object? parameters = null)
        {
            await using var connection = new NpgsqlConnection(connectionString);
            try
            {
                await connection.OpenAsync();


                var sql = BuildFunctionCall(functionName, parameters);

                return await connection.ExecuteScalarAsync<T>(sql, parameters);
            }
            catch (PostgresException ex)
            {
                throw new Exception($"Error PostgreSQL en '{functionName}': {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error general al ejecutar '{functionName}': {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<T>> ExecuteFunctionAsync<T>(string functionName, string connectionString, object? parameters = null)
        {
            await using var connection = new NpgsqlConnection(connectionString);
            try
            {
                await connection.OpenAsync();

                
                var sql = BuildFunctionCall(functionName, parameters);
                return await connection.QueryAsync<T>(sql, parameters);
            }
            catch (PostgresException ex)
            {
                throw new Exception($"Error PostgreSQL en '{functionName}': {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error general al ejecutar '{functionName}': {ex.Message}", ex);
            }
        }




        private static string BuildFunctionCall(string functionName, object? parameters)
        {
            if (parameters == null)
                return $"SELECT * FROM {functionName}()";

            var props = parameters.GetType().GetProperties();
            var paramNames = props.Select(p => $"@{p.Name}");
            var paramList = string.Join(", ", paramNames);

            return $"SELECT * FROM {functionName}({paramList})";
        }



    }
}
