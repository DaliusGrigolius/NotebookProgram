using NotebookProgram.Repository.DbContexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotebookProgram.Repository.Entities
{
    public class Note
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }
        public List<Image> Images { get; set; }
        public List<Category> Categories { get; set; }

        public Note(string title, string content)
        {
            Id = Guid.NewGuid();
            Title = title;
            Content = content;
            Categories = new List<Category>();
            Images = new List<Image>();
        }
    }
}
