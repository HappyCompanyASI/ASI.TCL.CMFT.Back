namespace ASI.TCL.CMFT.Application
{
    public interface IDomainUser
    {
        Guid Id { get; }
        string Account { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        bool IsActive { get; set; }
    }
}
