using System.ComponentModel.DataAnnotations;

namespace NotebookProgram.Repository.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Note> Notes { get; set; }

        public Category(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Notes = new List<Note>();
        }
    }
}
