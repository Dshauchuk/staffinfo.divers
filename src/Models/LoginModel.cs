using System.ComponentModel.DataAnnotations;

namespace Staffinfo.Divers.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Введите логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }
    }
}