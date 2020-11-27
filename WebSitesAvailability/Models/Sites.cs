using System;
using System.ComponentModel.DataAnnotations;

namespace WebSitesAvailability.Models
{
    public class Sites
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указано название сайта!")]
        [DataType(DataType.Url)]
        [RegularExpression(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$", ErrorMessage = @"Пожалуйста,введите корректный Url!")]
        [Display(Name = "Название сайта")]
        public string Url { get; set; }

        [Display(Name = "Дата последней проверки")]
        public DateTime CheckDate { get; set; }


        [Display(Name = "Доступность")]
        public bool IsAvailable { get; set; }
    }
}
