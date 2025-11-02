using DevicesApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace DevicesApi.Data
{
    // This class works as a bridge between our code and the SQL Server database.

    public class DevicesDbContext : DbContext
    {

        public DevicesDbContext(DbContextOptions<DevicesDbContext> options)
            : base(options)
        {
        }

        // This DbSet will map to a "Devices" table in SQL Server.
        public DbSet<Device> Devices { get; set; }

        //If needed we can fine-tune EF Core behaviour here if needed later (table names, keys, etc.)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ensure string lengths match our attributes in Device.cs
            modelBuilder.Entity<Device>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Brand).HasMaxLength(100).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });
        }
    }
}
