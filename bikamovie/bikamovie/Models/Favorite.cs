namespace bikamovie.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public int FilmId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public Film Film { get; set; }
        public User User { get; set; }
    }
}
