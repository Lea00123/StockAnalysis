using Microsoft.EntityFrameworkCore;

namespace StockAnalysis.Models
{
    public class StockContext : DbContext
    {
        // Define database tables
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockPrice> StockPrices { get; set; }

        // OnConfiguring is Entity Framework method - sets up database connection
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=.;Database=StockAnalysisDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        // OnModelCreating is Entity Framework method - sets up relationships between tables
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set composite primary key for Stock
            modelBuilder.Entity<Stock>()
                .HasKey(s => new { s.ISIN, s.Country });

            // Configure one-to-many relationship between Stock and StockPrice
            modelBuilder.Entity<StockPrice>()
                .HasOne(sp => sp.Stock)                // Each StockPrice has one Stock
                .WithMany(s => s.StockPrices)          // One Stock has many StockPrices
                .HasForeignKey(sp => new { sp.ISIN, sp.Country });  // Foreign key fields
        }
    }
}