namespace ASI.TCL.CMFT.Domain
{
    public interface IInternalEventHandler
    {
        void Handle(object @event);
    }
}