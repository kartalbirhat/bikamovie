using System;
using System.ComponentModel.DataAnnotations;

namespace bikamovie.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int FilmId { get; set; }
        public Film Film { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}