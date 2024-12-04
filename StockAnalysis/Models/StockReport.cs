namespace StockAnalysis.Models
{
    public class StockReport
    {
        public string Ticker { get; set; }
        public string Country { get; set; }
        public decimal MarketCap { get; set; }
        public decimal AveragePrice { get; set; }
        public long TotalVolume { get; set; }
        public string PriceTrend { get; set; }
    }
}

