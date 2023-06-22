using Microsoft.EntityFrameworkCore;
using VillaAPI.Models;

namespace VillaAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<LocalUser> LocalUsers { get; set; }
        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Name = "Royal Villa",
                    Details = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. ",
                    ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa1.jpg",
                    Occupancy = 5,
                    Rate = 200,
                    Sqft = 550,
                    Amenity = "Pool",
                    CreatedDate = DateTime.Now,
                },
                new Villa()
                {
                    Id = 2,
                    Name = "Bliss Villa",
                    Details = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. ",
                    ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa2.jpg",
                    Occupancy = 5,
                    Rate = 200,
                    Sqft = 550,
                    Amenity = "Pool",
                    CreatedDate = DateTime.Now,
                },
                new Villa()
                {
                    Id = 3,
                    Name = "Diamond Villa",
                    Details = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. ",
                    ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa3.jpg",
                    Occupancy = 5,
                    Rate = 200,
                    Sqft = 550,
                    Amenity = "Pool",
                    CreatedDate = DateTime.Now,
                },
                new Villa()
                {
                    Id = 4,
                    Name = "Jade Villa",
                    Details = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. ",
                    ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa4.jpg",
                    Occupancy = 5,
                    Rate = 200,
                    Sqft = 550,
                    Amenity = "Pool",
                    CreatedDate = DateTime.Now,
                },
                new Villa()
                {
                    Id = 5,
                    Name = "Opal Villa",
                    Details = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. ",
                    ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa5.jpg",
                    Occupancy = 5,
                    Rate = 200,
                    Sqft = 550,
                    Amenity = "Pool",
                    CreatedDate = DateTime.Now,
                }
            );
        }
    }
}
