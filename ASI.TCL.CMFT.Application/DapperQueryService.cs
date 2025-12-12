using System.Data.Common;
using Dapper;

namespace ASI.TCL.CMFT.Application
{
    public class QueryService : IQueryService
    {
        private readonly Func<DbConnection> _connectionFactory;

        public QueryService(Func<DbConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null)
        {
            await using var connection = _connectionFactory();
            await connection.OpenAsync();
            return await connection.QueryAsync<T>(sql, parameters);
        }

        public async Task<T?> QuerySingleAsync<T>(string sql, object? parameters = null)
        {
            await using var connection = _connectionFactory();
            await connection.OpenAsync();
            return await connection.QuerySingleOrDefaultAsync<T>(sql, parameters);
        }

        public async Task<int> ExecuteAsync(string sql, object? parameters = null)
        {
            await using var connection = _connectionFactory();
            await connection.OpenAsync();
            return await connection.ExecuteAsync(sql, parameters);
        }
    }
}