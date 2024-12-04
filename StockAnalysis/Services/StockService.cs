using System.IO;                    // For reading files
using System.Text.Json;              // For JSON processing
using StockAnalysis.Models;          // For Stock and StockContext

namespace StockAnalysis.Services
{
    public class StockService
    {
        // Store database context
        private readonly StockContext _context;

        // Constructor - gets database context
        public StockService(StockContext context)
        {
            _context = context;
        }

        // Method to import JSON data into database from files
        public async Task ImportJsonData(string stocksJson, string pricesJson)
        {
            // Deserialize JSON data
            var stocks = JsonSerializer.Deserialize<List<Stock>>(stocksJson);
            var priceData = JsonSerializer.Deserialize<List<StockPriceJson>>(pricesJson);

            // Clear existing data first
            _context.Stocks.RemoveRange(_context.Stocks);
            _context.StockPrices.RemoveRange(_context.StockPrices);
            await _context.SaveChangesAsync();

            // Add new stock data to database
            await _context.Stocks.AddRangeAsync(stocks);
            await _context.SaveChangesAsync();

            // Add stock price data to database
            foreach (var priceInfo in priceData)
            {
                int priceCount = priceInfo.Prices?.Count ?? 0;
                int volumeCount = priceInfo.Volume?.Count ?? 0;

                for (int i = 0; i < priceCount; i++)
                {
                    if (priceInfo.Prices[i].HasValue && i < volumeCount)
                    {
                        var stockPrice = new StockPrice
                        {
                            ISIN = priceInfo.ISIN,
                            Country = priceInfo.Country,
                            Price = priceInfo.Prices[i],
                            Volume = priceInfo.Volume[i]
                        };
                        await _context.StockPrices.AddAsync(stockPrice);
                    }
                    else if (priceInfo.Prices[i].HasValue && i >= volumeCount)
                    {
                        // Handle case where volume is missing
                        var stockPrice = new StockPrice
                        {
                            ISIN = priceInfo.ISIN,
                            Country = priceInfo.Country,
                            Price = priceInfo.Prices[i],
                            Volume = null // Volume is missing; you can decide how to handle this
                        };
                        await _context.StockPrices.AddAsync(stockPrice);
                    }
                }
            }

            await _context.SaveChangesAsync(); // Commit changes to database
        }
    }
}
