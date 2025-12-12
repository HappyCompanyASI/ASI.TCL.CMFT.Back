using ASI.TCL.CMFT.Domain;

namespace ASI.TCL.CMFT.Infrastructure.EFCore
{
    public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
    {
        public async Task CommitAsync(CancellationToken cancellationToken = default) 
            => await dbContext.SaveChangesAsync(cancellationToken);
    }
}