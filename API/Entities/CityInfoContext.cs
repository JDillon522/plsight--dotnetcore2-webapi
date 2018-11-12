using Microsoft.EntityFrameworkCore;
using API.Entities;

namespace API.Entities
{
    public class CityInfoContext : DbContext
    {
        public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
        {
            Database.Migrate();
        }
        public DbSet<City> Cities { get; set; }
        public DbSet<Point> Points { get; set; }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseSqlServer("connectionString");

        //     base.OnConfiguring(optionsBuilder);
        // }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            #region CitySeed
            modelBuilder.Entity<City>().HasData(new City {
                Id = 1,
                Name = "Seeded City Name",
                Description = "Lets see if this works"
            });
            #endregion

            modelBuilder.Entity<Point>(entity =>
            {
                entity.Property(d => d.Name).IsRequired();
            });

            #region PointSeed
            modelBuilder.Entity<Point>().HasData(
                new Point() {
                    Id = 1,
                    CityId = 1,
                    Name = "First Point",
                    Description = "Test 1"
                });
            #endregion

            #region AnonymousPointSeed
            modelBuilder.Entity<Point>().HasData(
                new {
                    Id = 2,
                    CityId = 1,
                    Name = "Second Point",
                    Description = "Test 2"
                });
            #endregion
        }

    }
}