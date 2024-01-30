using BulkyWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BulkyWeb.Data
{
	//any random name can be used instead of class ApplicationDbContext
	public class ApplicationDbContext : DbContext
	{
        //implementing DbContext so that we can access entity framework in our project

        //DbContextOptions<ApplicationDbContext> options) : base(options) --> mentioning that ApplicationDb
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }

        //OnModelCreating method is overriden to seed data and define relationships betn tables, specifying constraints, etc.
        //ModelBuilder is a class that provides various methods do all this stuff and directly interact with Db
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, DisplayOrder = 1, Name = "Action" },
                new Category { CategoryId = 2, DisplayOrder = 2, Name = "History" },
                new Category { CategoryId = 3, DisplayOrder = 3, Name = "Sci-Fi" }
            );
        }
    }
}
