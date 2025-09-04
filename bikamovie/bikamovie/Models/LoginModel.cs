using System.ComponentModel.DataAnnotations;

namespace bikamovie.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Kullan�c� ad� zorunludur.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "�ifre zorunludur.")]
        public string Password { get; set; }
    }
}