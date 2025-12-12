using ASI.TCL.CMFT.Domain;

namespace ASI.TCL.CMFT.Infrastructure.EFCore
{
    public interface IApplicationDbContext
    {
        Task AddAsync<T>(T entity) where T : class;
        Task<bool> ExistsAsync<T, TId>(TId id) where T : AggregateRoot<TId>;
        Task<T> LoadAsync<T, TId>(TId id) where T : AggregateRoot<TId>;
        void Remove<T>(T entity) where T : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}