using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Book
    {
        public int BookId { get; set; }

        [Required]
        [Display(Name = "Название")]
        public string Title { get; set; }
        [Display(Name = "Описание")]
        public string Annotation { get; set; }
        public int? AuthorRefId { get; set; }
        public Author Author { get; set; } = null;
    }
}
