using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Tchat.Api.Domain
{
    public class User: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [AllowNull]
        public DateOnly? Birthdate { get; set; }
        [AllowNull]
        public string? PicturePath { get; set; }
        public string? Bio { get; set; }
        public bool IsDeleted { get; set; }
        [AllowNull]
        public DateTime? DeletedOn { get; set; }
        public bool IsBanned { get; set; }
        [AllowNull]
        public DateTime? BannedOn { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
