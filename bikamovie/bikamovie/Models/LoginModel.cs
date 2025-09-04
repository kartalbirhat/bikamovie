using System.ComponentModel.DataAnnotations;

namespace bikamovie.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Kullanýcý adý zorunludur.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Þifre zorunludur.")]
        public string Password { get; set; }
    }
}