using Microsoft.EntityFrameworkCore;
using api_test2.Models;

namespace api_test2.Data
{
    public class RouletteContext : DbContext
    {
        public RouletteContext(DbContextOptions<RouletteContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasKey(u => u.Nombre);
            modelBuilder.Entity<Usuario>().Property(u => u.Nombre).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Usuario>().Property(u => u.Monto).IsRequired();
        }
    }
}
