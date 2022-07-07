using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotebookProgram.Repository.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public UserDetails UserDetails { get; set; }
        public List<Note> Notes { get; set; }
        public List<Category> Categories { get; set; }

        public User(string email, string password, UserDetails userDetails)
        {
            Id = Guid.NewGuid();
            Email = email;
            Password = password;
            UserDetails = userDetails;
            Notes = new List<Note>();
            Categories = new List<Category>();
        }
    }
}
