using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataBasesAndModels.Models
{
    public class Character
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Обязательное поле!!!")]
        [StringLength(16, MinimumLength = 1, ErrorMessage = "От 1 до 16 символов!")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Обязательное поле!!!")]
        [StringLength(16, MinimumLength = 1, ErrorMessage = "От 1 до 16 символов!")]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Обязательное поле!!!")]
        [Range(10, 200, ErrorMessage = "Неверный возраст")]
        [Display(Name = "Возраст")]
        public int Age { get; set; }

        [Display(Name = "Пол")]
        public int Gender { get; set; }

        [Display(Name = "Отец")]
        public int FatherId { get; set; }

        [Display(Name = "Мать")]
        public int MotherId { get; set; }

        [Display(Name = "Пара")]
        public int CoupleId { get; set; }

        [Display(Name = "Номер дома")]
        public int HouseId { get; set; }

        [Display(Name = "Улица")]
        public int? StreetId { get; set; }

        public Street Street { get; set; }
    }
}