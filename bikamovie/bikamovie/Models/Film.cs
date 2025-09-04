using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bikamovie.Models
{
    public class Film
    {
        [Key]
        public int Id { get; set; }
        public string? Ad { get; set; }
        public string? Yonetmen { get; set; }
        public string? Konu { get; set; }
        public float? Sure { get; set; }
        public decimal? IMDb { get; set; }
        public string? Tur { get; set; }
        public int CikisYili { get; set; }
        public string? GorselUrl { get; set; }
        public ICollection<Comment> Comments { get; set; }  = new List<Comment>();
    }
}
