using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Web2_p1.Data
{
    public class BookAuthDbContext : IdentityDbContext
    {
        public BookAuthDbContext(DbContextOptions<BookAuthDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var readerRoleId = "004c7e80-7dfc-44be-8952-2c7130898655";
            var writeRoleId = "71e282d3-76ca-485e-b094-eff019287fa5";

            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = readerRoleId,
                    Name = "Read",
                    NormalizedName = "READ"
                },
                new IdentityRole
                {
                    Id = writeRoleId,
                    Name = "Write",
                    NormalizedName = "WRITE"
                });
        }
    }
}
