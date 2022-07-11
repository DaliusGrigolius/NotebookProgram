using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotebookProgram.Repository.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime Expires { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
