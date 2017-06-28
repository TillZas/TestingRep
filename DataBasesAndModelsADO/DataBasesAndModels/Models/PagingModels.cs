using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataBasesAndModels.Models
{

    public class PageInfo
    {
        public int PageNumber { get; set; } 
        public int PageSize { get; set; }
        public int TotalItems { get; set; } 
        public int TotalPages 
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }
    }

    public class CharacterViewModel
    {
        public IEnumerable<Character> Characters { get; set; }
        public PageInfo PageInfo { get; set; }
    }

    public class StreetViewModel
    {
        public IEnumerable<Street> Streets { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}