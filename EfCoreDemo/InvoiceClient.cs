using System.Text.Json;
using EfCoreDemo.Models;

internal class InvoiceClient
{
    private readonly HttpClient _httpClient;

    public InvoiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Invoice>> GetInvoicesAsync()
    {
        var response = await _httpClient.GetAsync("invoices");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<Invoice>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
}
