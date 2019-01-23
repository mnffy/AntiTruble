using Microsoft.EntityFrameworkCore;

namespace AntiTruble.Equipment.Models
{
    public partial class AntiTruble_EquipmentContext : DbContext
    {
        public AntiTruble_EquipmentContext()
        {
        }

        public AntiTruble_EquipmentContext(DbContextOptions<AntiTruble_EquipmentContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EquipmentDefects> EquipmentDefects { get; set; }
        public virtual DbSet<Equipments> Equipments { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity<EquipmentDefects>(entity =>
            {
                entity.HasKey(e => e.DefectId);

                entity.Property(e => e.DefectName).HasMaxLength(50);

                entity.Property(e => e.Price).HasColumnType("money");
            });

            modelBuilder.Entity<Equipments>(entity =>
            {
                entity.HasKey(e => e.EquipmentId);

                entity.Property(e => e.Name).HasMaxLength(50);
            });
        }
    }
}
