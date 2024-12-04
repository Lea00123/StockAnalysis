namespace StockAnalysis.Models
{
    public class Stock
    {
        public string ISIN { get; set; }
        public string Country { get; set; }
        public string Ticker { get; set; }
        public decimal MarketCap { get; set; }
        public virtual ICollection<StockPrice> StockPrices { get; set; }
    }
}