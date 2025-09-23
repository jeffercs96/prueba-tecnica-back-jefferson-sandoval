using Microsoft.EntityFrameworkCore;
using RouletteApi.Entities;
namespace RouletteApi.DbContext;

public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserWallet> Wallets => Set<UserWallet>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserWallet>()
            .HasIndex(u => u.Name)
            .IsUnique();

        modelBuilder.Entity<UserWallet>()
            .Property(u => u.Name)
            .HasColumnType("citext"); // case-insensitive en Postgres
    }

}