namespace ASI.TCL.CMFT.Application
{
    public interface IQueryService
    {
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null);
        Task<T?> QuerySingleAsync<T>(string sql, object? parameters = null);
        Task<int> ExecuteAsync(string sql, object? parameters = null);
    }

}