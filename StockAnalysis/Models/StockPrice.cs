using System.ComponentModel.DataAnnotations;

namespace StockAnalysis.Models
{
    public class StockPrice
    {
        [Key]
        public int PriceID { get; set; }
        public string ISIN { get; set; }
        public string Country { get; set; }
        public decimal? Price { get; set; }
        public int? Volume { get; set; }
        public virtual Stock Stock { get; set; }
    }
}