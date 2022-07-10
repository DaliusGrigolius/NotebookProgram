using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotebookProgram.Repository.Entities
{
    public class Image
    {
        public Guid Id { get; set; }
        [Required]
        public byte[] Byte { get; set; }
        [ForeignKey("Note")]
        public Guid NoteId { get; set; }
        public virtual Note Note { get; set; }

        public Image(byte[] @byte)
        {
            Id = Guid.NewGuid();
            Byte = @byte;
        }
    }
}
