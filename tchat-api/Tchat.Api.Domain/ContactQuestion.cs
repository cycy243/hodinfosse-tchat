using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace Tchat.Api.Domain
{
    public class ContactQuestion: IDeletable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string SenderEmail { get; set; }
        [Required]
        public string SenderName { get; set; }
        [Required]
        public string SenderFirstname { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime DateSend { get; set; }
        public DateTime? DateRespond { get; set; }
        public string? Response { get; set; }
        [NotMapped]
        public bool IsDeleted { get => DeletedOn != null; }
        public DateTime? DeletedOn { get; set; }
    }
}
