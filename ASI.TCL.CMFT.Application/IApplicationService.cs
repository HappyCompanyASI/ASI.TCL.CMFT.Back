namespace ASI.TCL.CMFT.Application
{
    public interface IApplicationService
    {
        Task Handle(object command);
    }
}