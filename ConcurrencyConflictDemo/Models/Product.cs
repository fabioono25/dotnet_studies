using System.ComponentModel.DataAnnotations;

namespace ConcurrencyConflictDemo.Models;
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; } = 0;
    public int Inventory { get; set; }
    
    // this property works as a concurrency token. Why? Because it's decorated with the [Timestamp] attribute.
    // objective: to prevent two users from updating the same record at the same time.
    [Timestamp]
    public byte[] RowVersion { get; set; }

    // another way o managing concurrency (aqpplication-managed concurrency token)
    // public Guid Version { get; set; }
}