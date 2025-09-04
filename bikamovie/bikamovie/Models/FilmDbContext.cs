using Microsoft.EntityFrameworkCore;
using bikamovie.Models; // Comment sınıfının olduğu namespace

public class FilmDbContext : DbContext
{
    public FilmDbContext(DbContextOptions<FilmDbContext> options)
        : base(options)
    {
    }

    public DbSet<Film> Filmler { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
}