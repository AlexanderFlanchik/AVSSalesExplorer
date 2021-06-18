using AVSSalesExplorer.Models;
using Microsoft.EntityFrameworkCore;

namespace AVSSalesExplorer.Services
{
    public class ItemDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemSize> Sizes { get; set; }
        public DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=items.db");
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);            
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasMany(i => i.Sizes)
                .WithOne(s => s.Item)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Item>()
                .HasMany(i => i.Sales)
                .WithOne(s => s.Item)
                .OnDelete(DeleteBehavior.Cascade);                
        }
    }
}