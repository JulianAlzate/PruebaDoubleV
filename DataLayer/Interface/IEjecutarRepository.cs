using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interface
{
    public interface IEjecutarRepository
    {
        Task<T> ExecuteScalarAsync<T>(string storedProcedure, string connectionString, object? parameters = null);
        Task<IEnumerable<T>> ExecuteFunctionAsync<T>(string functionName, string connectionString, object? parameters = null);
        //Task<IEnumerable<T>> ExecuteQueryAsync<T>(string storedProcedure, string connectionString, object? parameters = null);
        //Task<int> ExecuteNonQueryAsync(string storedProcedure, string connectionString, object? parameters = null);
    }
}
