using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        [Display(Name = "Имя писателя")]
        public string Name { get; set; }
    }
}
