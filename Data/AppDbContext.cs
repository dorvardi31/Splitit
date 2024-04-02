using Microsoft.EntityFrameworkCore;
using ImdbScraperApi.Models;

namespace ImdbScraperApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Actor> Actors { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
