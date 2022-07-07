using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotebookProgram.Repository.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsVisible { get; set; }
        public List<Note> Notes { get; set; }

        public Category(string name, bool isVisible = false)
        {
            Id = Guid.NewGuid();
            Name = name;
            IsVisible = isVisible;
            Notes = new List<Note>();
        }
    }
}
