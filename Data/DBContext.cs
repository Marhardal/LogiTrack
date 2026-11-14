using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack;

public class DBContext : IdentityDbContext<IdentityUser>
{
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .HasMany(order => order.Items)
            .WithOne(item => item.Order)
            .HasForeignKey(item => item.OrderID);
    
        modelBuilder.Entity<IdentityUser>()
            .HasIndex(user => user.UserName)
            .IsUnique();

        modelBuilder.Entity<IdentityRole>()
            .HasIndex(role => role.Name)
            .IsUnique();
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite("Data Source=logitrack.db");
}
