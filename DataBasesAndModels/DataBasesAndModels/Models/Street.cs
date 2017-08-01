using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataBasesAndModels.Models
{
    public class Street
    {

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Обязательное поле!!!")]
        [StringLength(16, MinimumLength = 1, ErrorMessage = "От 1 до 16 символов!")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Обязательное поле!!!")]
        [Range(-3000, 3000, ErrorMessage = "Вне нашего времени!!!")]
        [Display(Name = "Год создания")]
        public int CreationAge { get; set; }

        [HiddenInput(DisplayValue = false)]
        public ICollection<Character> Characters { get; set; }

        public Street()
        {
            Characters = new List<Character>();
        }
    }
}