using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataBasesAndModels.Models
{
    public class Street
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Character> Characters { get; set; }

        public Street()
        {
            Characters = new List<Character>();
        }
    }
}