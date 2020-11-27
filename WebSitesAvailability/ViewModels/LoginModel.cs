using System.ComponentModel.DataAnnotations;

namespace WebSitesAvailability.ViewModels
{
    public class LoginModel
    {
        [Display(Name = "��� ������������")]
        [Required(ErrorMessage = "�� ������� ��� ������������")]
        public string UserName { get; set; }

        [Display(Name = "������")]
        [Required(ErrorMessage = "�� ������ ������")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}