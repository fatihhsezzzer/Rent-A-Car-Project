using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentACar.Models.CarModels;
using RentACarProject.Models.UserModels;
using System.Reflection.Emit;


namespace RentACar.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        { }
        public DbSet<Car> Cars { get; set; }
        public DbSet<User> User { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"data source=(localdb)\MSSQLLocalDB;initial catalog=RentACar2;integrated security=true");
            //optionsBuilder.UseSqlServer(@"server=DESKTOP-JU3J93J;database=PMWADb;TrustServerCertificate=true;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Car>()
                   .Property(c => c.PricePerDay)
                   .HasColumnType("decimal(18, 2)");
        }
    }
}
