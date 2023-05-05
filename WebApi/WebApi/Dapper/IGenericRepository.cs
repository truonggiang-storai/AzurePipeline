using System.Data;

namespace WebApi.Dapper
{
    public interface IGenericRepository<T>
    {
        Task<int> AddAsync(T obj, int? timeout = null);

        Task<bool> UpdateAsync(T obj, int? timeout = null);

        Task<T> QuerySingleOrDefaultAsync(string sql, object? param = null, int? timeout = null);

        Task<Q> QuerySingleOrDefaultAsync<Q>(string sql, object? param = null, int? timeout = null) where Q : class;

        Task<T> QuerySingleOrDefaultAsync(IDbConnection dbConnection, string sql, object? param = null, int? timeout = null);

        Task<Q> QuerySingleOrDefaultAsync<Q>(IDbConnection dbConnection, string sql, object? param = null, int? timeout = null) where Q : class;

        Task<List<T>> QueryAsync(string sql, object? param = null, int? timeout = null);

        Task<List<Q>> QueryAsync<Q>(string sql, object? param = null, int? timeout = null) where Q : class;

        Task<List<T>> QueryAsync(IDbConnection dbConnection, string sql, object? param = null, int? timeout = null);

        Task<List<Q>> QueryAsync<Q>(IDbConnection dbConnection, string sql, object? param = null, int? timeout = null) where Q : class;
    }
}
