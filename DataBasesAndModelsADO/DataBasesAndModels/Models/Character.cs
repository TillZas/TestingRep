using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataBasesAndModels.Models
{
    public class Character
    {
        [HiddenInput (DisplayValue = false)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public int Age { get; set; }

        public int Gender { get; set; }

        public int FatherId { get;set; }

        public int MotherId { get; set; }

        public int CoupleId { get; set; }

        public int HouseId { get; set; }

        public int? StreetId { get; set; }

        public Street Street { get; set; }

    }
}