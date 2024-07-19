using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tchat.Api.Models
{
    public class ContactQuestionDto
    {
        public Guid Id { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string SenderFirstname { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public DateTime? DateSend { get; set; }
        public DateTime? DateRespond { get; set; }
        public string? Response { get; set; }
    }
}
