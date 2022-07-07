using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotebookProgram.Repository.Entities
{
    public class Note
    {
        public Guid Id { get; set; }
        [Required]
        public string Caption { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public bool IsVisible { get; set; }
        public List<Category> Category { get; set; }

        public Note(string caption, string content, bool isVisible = false)
        {
            Id = Guid.NewGuid();
            Caption = caption;
            Content = content;
            IsVisible = isVisible;
            Category = new List<Category>();
        }
    }
}
