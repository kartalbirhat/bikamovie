using System.Collections.Generic;

namespace bikamovie.Models
{
    public class FilmDetayViewModel
    {
        public Film Film { get; set; }
        public List<Comment> Comments { get; set; }
        public bool IsFavorite { get; set; }
    }
}