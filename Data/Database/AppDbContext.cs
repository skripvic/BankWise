using Data.Settings;
using Microsoft.EntityFrameworkCore;

namespace Data.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    { }
    public DbSet<Cassette> Cassettes { get; set; }
    public DbSet<Denomination> Denominations { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=atm.db");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Denomination>().HasKey(c => c.Id);
        modelBuilder.Entity<Cassette>().HasKey(c => c.Id);
        modelBuilder.Entity<Cassette>()
            .HasOne(c => c.Denomination)
            .WithMany()
            .HasForeignKey("DenominationId");
        modelBuilder.SeedData();
    }
}