using System.ComponentModel.DataAnnotations;

namespace NotebookProgram.Dto
{
    public class UserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
