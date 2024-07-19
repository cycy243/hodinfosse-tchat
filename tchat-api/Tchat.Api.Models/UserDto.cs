namespace Tchat.Api.Models
{
    public class UserDto
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? Birthdate { get; set; }
        public string? Token { get; set; }
        public string? Email { get; set; }
        public string? PicturePath { get; set; }
        public string? Bio { get; set; }
        public string? Password { get; set; }
        public IEnumerable<string>? Roles { get; set; }
    }
}
