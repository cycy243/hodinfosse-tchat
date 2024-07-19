using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Tchat.Api.Domain
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [NotNull]
        [Required]
        public string Content { get; set; }
        [NotNull]
        [Required]
        public DateTime SendDateTime { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        [NotNull]
        [Required]
        public string TchatIp { get; set; }
        public User User { get; set; }
    }
}
