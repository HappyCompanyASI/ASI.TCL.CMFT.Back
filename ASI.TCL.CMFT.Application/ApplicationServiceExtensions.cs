using ASI.TCL.CMFT.Domain;

namespace ASI.TCL.CMFT.Application
{
    public static class ApplicationServiceExtensions
    {
        public static async Task HandleCreate<T, TId>(this IApplicationService service, IAggregateStore store, IUnitOfWork unitOfWork, T aggregate)
            where T : AggregateRoot<TId>
        {
            await store.Save<T, TId>(aggregate);
            await unitOfWork.CommitAsync();
        }
        public static async Task HandleUpdate<T, TId>(this IApplicationService service, IAggregateStore store, IUnitOfWork unitOfWork, TId id, Action<T>? beforeSave)
            where T : AggregateRoot<TId>
        {
            var aggregate = await store.Load<T, TId>(id);
            if (beforeSave != null)  beforeSave(aggregate);
            await store.Save<T, TId>(aggregate);
            await unitOfWork.CommitAsync();
        }
        public static async Task HandleUpdate<T, TId>(this IApplicationService service, IAggregateStore store, IUnitOfWork unitOfWork, TId id, Func<T, Task> beforeSave)
            where T : AggregateRoot<TId>
        {
            var aggregate = await store.Load<T, TId>(id);
            await beforeSave(aggregate);
            await store.Save<T, TId>(aggregate);
            await unitOfWork.CommitAsync();
        }
        public static async Task HandleDelete<T, TId>(this IApplicationService service, IAggregateStore store, IUnitOfWork unitOfWork, TId id, Action<T>? beforeRemove = null)
            where T : AggregateRoot<TId>
        {
            var aggregate = await store.Load<T, TId>(id);
            if (beforeRemove != null) beforeRemove(aggregate);
            await store.Remove<T, TId>(aggregate);
            await unitOfWork.CommitAsync();
        }
        public static async Task HandleDelete<T, TId>(this IApplicationService service, IAggregateStore store, IUnitOfWork unitOfWork, TId id, Func<T, Task> beforeRemove)
            where T : AggregateRoot<TId>
        {
            var aggregate = await store.Load<T, TId>(id);
            await beforeRemove(aggregate);
            await store.Remove<T, TId>(aggregate);
            await unitOfWork.CommitAsync();
        }
    }
}