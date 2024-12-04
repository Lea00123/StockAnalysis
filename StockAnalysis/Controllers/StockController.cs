using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockAnalysis.Services;
using System.IO;
using System.Threading.Tasks;

namespace StockAnalysis.Controllers
{
    [ApiController]
    [Route("api/Stock")]
    public class StockController : ControllerBase  // Inherits API controller functionality
    {
        private readonly StockService _stockService;

        public StockController(StockService stockService)
        {
            _stockService = stockService;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportData(IFormFile stocksFile, IFormFile pricesFile)
        {
            // Check if the files are provided
            if (stocksFile == null || pricesFile == null)
            {
                return BadRequest("Both stock and price files are required.");
            }

            // Access the file names provided in the request (they will be automatically assigned by the client, such as 'stocks.json' and 'stock_prices.json')
            string stocksFileName = stocksFile.FileName;
            string pricesFileName = pricesFile.FileName;

            // Optionally, log the names or validate them
            Console.WriteLine($"Received files: {stocksFileName} and {pricesFileName}");

            // Read the content of the files (you can use a helper method like ReadFileAsync to convert file content to string)
            var stocksJson = await ReadFileAsync(stocksFile);
            var pricesJson = await ReadFileAsync(pricesFile);

            // Call the service to import the data
            await _stockService.ImportJsonData(stocksJson, pricesJson);

            return Ok("Data imported successfully");
        }

        // Helper method to read file content asynchronously
        private async Task<string> ReadFileAsync(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
