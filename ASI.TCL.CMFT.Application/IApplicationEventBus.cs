namespace ASI.TCL.CMFT.Application
{
    public interface IApplicationEventBus
    {
        void Publish(IApplicationEvent evt);
        void Subscribe(Action<IApplicationEvent> receiver);
        void Unsubscribe(Action<IApplicationEvent> receiver);
    }

}
