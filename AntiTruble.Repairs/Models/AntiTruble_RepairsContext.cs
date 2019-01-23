using Microsoft.EntityFrameworkCore;

namespace AntiTruble.Repairs.Models
{
    public partial class AntiTruble_RepairsContext : DbContext
    {
        public AntiTruble_RepairsContext()
        {
        }

        public AntiTruble_RepairsContext(DbContextOptions<AntiTruble_RepairsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Repairs> Repairs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity<Repairs>(entity =>
            {
                entity.HasKey(e => e.RepairId);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");
            });
        }
    }
}
