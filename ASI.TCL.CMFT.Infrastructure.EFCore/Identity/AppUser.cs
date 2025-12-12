using System.ComponentModel.DataAnnotations.Schema;
using ASI.TCL.CMFT.Application;
using Microsoft.AspNetCore.Identity;

namespace ASI.TCL.CMFT.Infrastructure.EFCore.Identity
{
    public class AppUser : IdentityUser<Guid>, IDomainUser, IAuditable
    {
        [NotMapped]
        public string Account
        {
            get => UserName;
            set => UserName = value;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BelongUnit { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}