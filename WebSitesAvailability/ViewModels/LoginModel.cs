using System.ComponentModel.DataAnnotations;

namespace WebSitesAvailability.ViewModels
{
    public class LoginModel
    {
        [Display(Name = "Имя пользователя")]
        [Required(ErrorMessage = "Не указано имя пользователя")]
        public string UserName { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}