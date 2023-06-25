using CrispyRazor.Models;
using Microsoft.EntityFrameworkCore;

namespace CrispyRazor.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        public DbSet<Categories> Category { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categories>().HasData(
                new Categories { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Categories { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                new Categories { Id = 3, Name = "History", DisplayOrder = 3 }
                );
        }
        public void RemoveRecord(Type category)
        {
            Remove(category);
            SaveChanges();
        }
    }
}
