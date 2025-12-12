namespace ASI.TCL.CMFT.Application
{
    public interface IRequestHandler
    {
        Task HandleCommand<TCommand>(TCommand command, Func<TCommand, Task> handler);
        Task<TResult> HandleQuery<TResult>(Func<Task<TResult>> query);
    }
}