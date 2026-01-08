using Microsoft.EntityFrameworkCore;
using WEB_253551_KORZUN.Domain.Entities;

namespace WEB_253551_KORZUN.API.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

    public DbSet<CarPart> CarParts { get; set; }
    public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CarPart>()
                .HasOne(cp => cp.Category)
                .WithMany(c => c.CarParts)
                .HasForeignKey(cp => cp.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
