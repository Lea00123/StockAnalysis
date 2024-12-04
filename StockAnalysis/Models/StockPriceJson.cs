using System.Collections.Generic;

namespace StockAnalysis.Models
{
    public class StockPriceJson
    {
        public string ISIN { get; set; }
        public string Country { get; set; }
        public List<decimal?> Prices { get; set; }
        public List<int?> Volume { get; set; }
    }
}