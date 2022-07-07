using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotebookProgram.Repository.Entities
{
    public class Image
    {
        public Guid Id { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public string Alt { get; set; }

        public Image(string url, string alt)
        {
            Id = Guid.NewGuid();
            Url = url;
            Alt = alt;
        }
    }
}
