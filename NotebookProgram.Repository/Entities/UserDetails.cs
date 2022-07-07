using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotebookProgram.Repository.Entities
{
    public class UserDetails
    {
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }

        public UserDetails(string firstName, string lastName, DateTime birthDate, string? gender = null, string? address = null)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Gender = gender;
            Address = address;
        }
    }
}
