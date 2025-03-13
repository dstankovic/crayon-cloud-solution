using CloudSales.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudSales.Infrastructure.Persistence;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<SoftwareService> SoftwareServices { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Subscription>()
           .HasKey(s => new { s.AccountId, s.SoftwareServiceId });

        base.OnModelCreating(modelBuilder);
    }
}
