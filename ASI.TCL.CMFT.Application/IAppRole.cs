namespace ASI.TCL.CMFT.Application
{
    public interface IDomainRole
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        bool IsActive { get; set; }
    }
}

 