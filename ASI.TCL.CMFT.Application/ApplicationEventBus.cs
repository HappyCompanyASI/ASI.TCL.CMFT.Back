namespace ASI.TCL.CMFT.Application
{
    public class ApplicationEventBus : IApplicationEventBus
    {
        private readonly List<Action<IApplicationEvent>> _handlers = new();
        private readonly object _lock = new(); // thread-safe 用

        public void Publish(IApplicationEvent notification)
        {
            List<Action<IApplicationEvent>> subscribers;

            lock (_lock)
            {
                subscribers = _handlers.ToList(); // 建立快照，避免執行中被改
            }

            foreach (var handler in subscribers)
            {
                handler?.Invoke(notification);
            }
        }

        public void Subscribe(Action<IApplicationEvent> handler)
        {
            lock (_lock)
            {
                if (!_handlers.Contains(handler))
                    _handlers.Add(handler);
            }
        }

        public void Unsubscribe(Action<IApplicationEvent> handler)
        {
            lock (_lock)
            {
                _handlers.Remove(handler);
            }
        }
    }
}