using System.ComponentModel.DataAnnotations;

namespace bikamovie.Models
{
    public class RegisterModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        [StringLength(50)]
        public string? Username { get; set; }

        [Required(ErrorMessage = "E-Posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string? Password { get; set; }
    }
}
