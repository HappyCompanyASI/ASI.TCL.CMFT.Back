namespace ASI.TCL.CMFT.Domain
{
    public abstract class AggregateRoot<TId> : IInternalEventHandler
    {
        public TId Id { get; protected set; }
        private readonly List<object> _changes;

        protected AggregateRoot() => _changes = new List<object>();

        protected void Apply(object @event)
        {
            When(@event);              // 執行狀態改變
            EnsureValidState();        // 確保狀態合法
            _changes.Add(@event);      // 記錄事件
        }
      
        public IEnumerable<object> GetChanges() => _changes.AsEnumerable();
        public void ClearChanges() => _changes.Clear();
        protected void ApplyToEntity(IInternalEventHandler entity, object @event)
            => entity?.Handle(@event);

        void IInternalEventHandler.Handle(object @event) => When(@event);
        protected abstract void When(object @event);
        protected abstract void EnsureValidState();
    }
}