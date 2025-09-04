using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bikamovie.Models
{
    public class ProfileModel
    {
        [Display(Name = "Kullan�c� Ad�")]
        public string? UserName { get; set; }

        [Display(Name = "E-posta")]
        public string? Email { get; set; }
        public List<Film> FavoriteFilms { get; set; } = new();
    }
}