using BulkieWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkieWeb.Data
{
    /* Basic configuration for entity framework */
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

               
        }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Sci-Fi", DisplayOrder = 1},
                new Category { Id = 2, Name = "Horror", DisplayOrder = 2},
                new Category { Id = 3, Name = "Romance", DisplayOrder = 3});
        }
    }
}
