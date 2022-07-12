using System.ComponentModel.DataAnnotations;

namespace NotebookProgram.Dto.Models
{
    public class UserLoginDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }
}
