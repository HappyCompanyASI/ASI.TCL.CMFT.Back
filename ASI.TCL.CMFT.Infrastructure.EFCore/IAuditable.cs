namespace ASI.TCL.CMFT.Infrastructure.EFCore
{
    public interface IAuditable
    {
        DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        DateTime UpdatedAt { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}