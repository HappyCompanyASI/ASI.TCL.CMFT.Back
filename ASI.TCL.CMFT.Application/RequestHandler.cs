namespace ASI.TCL.CMFT.Application
{
    //public class RequestHandler : IRequestHandler
    //{
    //    private readonly IOperatorAccessor _operatorAccessor;
    
    //    public RequestHandler(IOperatorAccessor operatorAccessor)
    //    {
    //        _operatorAccessor = operatorAccessor;
    //    }

    //    public async Task HandleCommand<TCommand>(TCommand command, Func<TCommand, Task> handler)
    //    {
    //        var attr = AuthorityAttributeHelper.GetAuthorityAttribute(typeof(TCommand));

    //        if (attr != null)//若Command沒有指定權限Attribute則直接執行handler
    //        {
    //            var required = attr.Code;
    //            var current = _operatorAccessor.Current;

    //            if (!current.HasAuthority(required))
    //                throw new UnauthorizedAccessException($"未具備權限：{required}");
    //        }

    //        await handler(command);
    //    }

    //    public async Task<TResult> HandleQuery<TResult>(Func<Task<TResult>> query)
    //    {
    //        return await query();
    //    }
    //}
}