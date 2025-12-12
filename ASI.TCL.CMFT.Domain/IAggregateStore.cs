namespace ASI.TCL.CMFT.Domain
{
    public interface IAggregateStore
    {
        Task<bool> Exists<T, TId>(TId aggregateId) where T : AggregateRoot<TId>;
        Task<T> Load<T, TId>(TId aggregateId) where T : AggregateRoot<TId>;
        Task Save<T, TId>(T aggregate) where T : AggregateRoot<TId>;
        Task Remove<T, TId>(T aggregate) where T : AggregateRoot<TId>;
    }
}