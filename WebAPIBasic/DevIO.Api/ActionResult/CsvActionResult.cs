using System.Globalization;
using System.Text;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.Api.ActionResult
{
    public class CsvActionResult<T> : IActionResult where T : new()
    {
        private readonly  IEnumerable<T> _data;

        public CsvActionResult(IEnumerable<T> data)
        {
            _data = data;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.ContentType = "text/csv";

            await using var writer = new StreamWriter(context.HttpContext.Response.Body);
            await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            // Write CSV headers dynamically based on the properties of the data type
            await csv.WriteRecordsAsync(new[] { new T() });

            foreach (var item in _data)
            {
                // Write each item's data in CSV format
                csv.WriteRecord(item);
                await writer.WriteLineAsync();
            }
        }
    }
}
