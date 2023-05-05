using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace WebApi.Dapper
{
    public class DapperRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public DapperRepository(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<int> AddAsync(T obj, int? timeout = null)
        {
            using (IDbConnection connection = _sqlConnectionFactory.GetNewConnection())
            {
                return await connection.InsertAsync(obj, null, timeout).ConfigureAwait(false);
            }
        }

        public async Task<bool> UpdateAsync(T obj, int? timeout = null)
        {
            using (IDbConnection connection = _sqlConnectionFactory.GetNewConnection())
            {
                return await connection.UpdateAsync(obj, null, timeout).ConfigureAwait(false);
            }
        }

        public async Task<List<T>> QueryAsync(string sql, object? param = null, int? timeout = null)
        {
            using (IDbConnection connection = _sqlConnectionFactory.GetNewConnection())
            {
                var query = await QueryAsync(connection, sql, param).ConfigureAwait(false);
                return query.AsList();
            }
        }

        public async Task<List<T>> QueryAsync(IDbConnection dbConnection, string sql, object? param = null, int? timeout = null)
        {
            var query = await dbConnection.QueryAsync<T>(sql, param, null, timeout).ConfigureAwait(false);
            return query.AsList();
        }

        public async Task<List<Q>> QueryAsync<Q>(IDbConnection dbConnection, string sql, object? param = null, int? timeout = null) where Q : class
        {
            var query = await dbConnection.QueryAsync<Q>(sql, param, null, timeout);
            return query.AsList();
        }

        public async Task<List<Q>> QueryAsync<Q>(string sql, object? param = null, int? timeout = null) where Q : class
        {
            using (IDbConnection connection = _sqlConnectionFactory.GetNewConnection())
            {
                var query = await QueryAsync<Q>(connection, sql, param, timeout).ConfigureAwait(false);
                return query.AsList();
            }
        }

        public async Task<T> QuerySingleOrDefaultAsync(IDbConnection dbConnection, string sql, object? param = null, int? timeout = null)
        {
            return await dbConnection.QuerySingleOrDefaultAsync<T>(sql, param, null, timeout).ConfigureAwait(false);
        }

        public async Task<T> QuerySingleOrDefaultAsync(string sql, object? param = null, int? timeout = null)
        {
            using (IDbConnection connection = _sqlConnectionFactory.GetNewConnection())
            {
                return await QuerySingleOrDefaultAsync(connection, sql, param, timeout).ConfigureAwait(false);
            }
        }

        public async Task<Q> QuerySingleOrDefaultAsync<Q>(IDbConnection dbConnection, string sql, object? param = null, int? timeout = null) where Q : class
        {
            return await dbConnection.QuerySingleOrDefaultAsync<Q>(sql, param, null, timeout);
        }

        public async Task<Q> QuerySingleOrDefaultAsync<Q>(string sql, object? param = null, int? timeout = null) where Q : class
        {
            using (IDbConnection connection = _sqlConnectionFactory.GetNewConnection())
            {
                return await QuerySingleOrDefaultAsync<Q>(connection, sql, param, timeout).ConfigureAwait(false);
            }
        }
    }
}
