using ASI.TCL.CMFT.Application;
using Microsoft.AspNetCore.Identity;

namespace ASI.TCL.CMFT.Infrastructure.EFCore.Identity
{
    public class AppRole : IdentityRole<Guid>, IDomainRole, IAuditable
    {
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}