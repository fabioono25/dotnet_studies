using FluentAssertions;
using InvoiceApp.WebApi;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;

namespace InvoiceApp.IntegrationTests;
public class WeatherForecastApiTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    // what is WebApplicationFactory?
    // WebApplicationFactory is a class provided by ASP.NET Core that creates an in-memory test server for the application under test.
    // It is used to create an instance of the application that can be used to make HTTP requests to the application.
    // The WebApplicationFactory class is a generic class that takes the type of the application’s entry point as a type parameter.
    // In this case, the entry point of the application is the Program class, so we pass Program as the type parameter to WebApplicationFactory.

    [Fact]
    public async Task GetWeatherForecast_ReturnsSuccessAndCorrectContentType()
    {
        // Arrange
        var client = factory.CreateClient();
        // Act
        var response = await client.GetAsync("/WeatherForecast");
        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        // Deserialize the response
        var responseContent = await response.Content.ReadAsStringAsync();
        var weatherForecast = JsonSerializer.Deserialize<List<WeatherForecast>>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        weatherForecast.Should().NotBeNull();
        weatherForecast.Should().HaveCount(5);
    }
}
