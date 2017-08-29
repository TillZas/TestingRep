using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class LibraryModels
    {
        public class Author
        {
            public int AuthorId { get; set; }
            public string Name { get; set; }
        }
        public class Book
        {
            public int BookId { get; set; }

            [Required]
            public string Title { get; set; }
            public string Annotation { get; set; }
            public int? AuthorRefId { get; set; }
            public Author Author { get; set; } = null;
        }
    }
}
