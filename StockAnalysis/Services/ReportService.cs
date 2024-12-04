using StockAnalysis.Models; // For StockReport and DbContext
using Microsoft.EntityFrameworkCore; // For EF Core extensions
using System.Linq;

namespace StockAnalysis.Services
{
    public class ReportService
    {
        private readonly StockContext _context;

        // Constructor to inject StockContext
        public ReportService(StockContext context)
        {
            _context = context;
        }

        public async Task<List<StockReport>> GetStockReport()
        {
            var result = await (
                from stock in _context.Stocks
                join price in _context.StockPrices
                    on new { stock.ISIN, stock.Country } equals new { price.ISIN, price.Country }
                group price by new { stock.Ticker, stock.Country, stock.MarketCap } into g
                select new StockReport
                {
                    Ticker = g.Key.Ticker,
                    Country = g.Key.Country,
                    MarketCap = g.Key.MarketCap,
                    AveragePrice = g.Average(p => p.Price ?? 0),  // Handle null prices
                    TotalVolume = g.Sum(p => p.Volume ?? 0),      // Handle null volumes
                    PriceTrend = g.OrderBy(p => p.PriceID).FirstOrDefault().Price < g.OrderBy(p => p.PriceID).LastOrDefault().Price ? "Up" : "Down" // Price trend based on ordered prices
                }
            ).OrderByDescending(r => r.MarketCap).ToListAsync(); // Sort by market cap descending

            return result;
        }
    }
}
