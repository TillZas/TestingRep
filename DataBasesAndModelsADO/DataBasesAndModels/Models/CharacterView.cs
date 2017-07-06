using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DataBasesAndModels.Models
{
    public class CharacterView
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Имя:")]
        public string Name { get; set; }

        [Display(Name = "Фамилия:")]
        public string Surname { get; set; }

        [Display(Name = "Возраст:")]
        public int Age { get; set; }

        [Display(Name = "Пол:")]
        public string Gender { get; set; }

        [Display(Name = "Отец:")]
        public string FatherName { get; set; }

        [Display(Name = "Мать:")]
        public string MotherName { get; set; }

        [Display(Name = "Пара:")]
        public string CoupleName { get; set; }

        [Display(Name = "Домашний адрес:")]
        public string HouseName { get; set; }
    }
}