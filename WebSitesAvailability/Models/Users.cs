using System.ComponentModel.DataAnnotations;

namespace WebSitesAvailability.Models
{
    public class Users
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
