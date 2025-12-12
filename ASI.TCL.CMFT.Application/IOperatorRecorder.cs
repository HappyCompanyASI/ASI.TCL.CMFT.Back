namespace ASI.TCL.CMFT.Application
{
    public interface IOperatorRecorder
    {
        Task RecordAsync(object @event);
    }
}