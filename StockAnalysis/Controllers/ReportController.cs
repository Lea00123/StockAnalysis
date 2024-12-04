using Microsoft.AspNetCore.Mvc;
using StockAnalysis.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using StockAnalysis.Models; // For StockReport

namespace StockAnalysis.Controllers
{
    [ApiController]
    [Route("api/Stock/Reports")]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _reportService;

        // Constructor to inject ReportService
        public ReportController(ReportService reportService)
        {
            _reportService = reportService;
        }

        // GET endpoint to retrieve stock report
        [HttpGet]
        public async Task<IActionResult> GetStockReport()
        {
            // Get the stock report
            List<StockReport> reports = await _reportService.GetStockReport();

            // Return the report data as JSON
            return Ok(reports);
        }
    }
}
