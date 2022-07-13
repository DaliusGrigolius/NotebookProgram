using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotebookProgram.Repository.Entities
{
    public class Image
    {
        public Guid Id { get; set; }
        [Required]
        public byte[] Byte { get; set; }
        [ForeignKey("Note")]
        public Guid NoteId { get; set; }
        public Note Note { get; set; }

        public Image(byte[] @byte)
        {
            Id = Guid.NewGuid();
            Byte = @byte;
        }
    }
}
