namespace Tchat.Api.Models
{
    public class MessageDto
    {
        public Guid? Id { get; set; }
        public string Content { get; set; }
        public DateTime SendDateTime { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
    }
}
