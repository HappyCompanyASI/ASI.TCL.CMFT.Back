namespace ASI.TCL.CMFT.Domain
{
    public interface IRepository<T, in TId> where T : AggregateRoot<TId>
    {
        Task<T?> Load(TId id);
        Task Add(T entity);
        Task<bool> Exists(TId id);
        void Remove(T user);
    }
}