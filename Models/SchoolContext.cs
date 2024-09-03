using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
namespace WebApplication1.Models
{

    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            modelBuilder.Entity<Student>()
                .Property(s => s.name)
                .IsRequired()  
                .HasMaxLength(100);  

            
        }




    }

}

