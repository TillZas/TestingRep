using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DataBasesAndModels.Models
{
    public class TownContext :DbContext
    {


        public DbSet<Character> Characters { get; set; }
        public DbSet<Street> Streets { get; set; }
    }


}