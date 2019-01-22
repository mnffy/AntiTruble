using Microsoft.EntityFrameworkCore;

namespace AntiTruble.Repairs.Models
{
    public partial class AntiTruble_PersonContext : DbContext
    {
        public AntiTruble_PersonContext()
        {
        }

        public AntiTruble_PersonContext(DbContextOptions<AntiTruble_PersonContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Persons> Persons { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity<Persons>(entity =>
            {
                entity.HasKey(e => e.PersonId);

                entity.Property(e => e.PersonId).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Balance).HasColumnType("money");

                entity.Property(e => e.DateBirth).HasColumnType("date");

                entity.Property(e => e.Fio)
                    .HasColumnName("FIO")
                    .HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(20);

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            });
        }
    }
}
