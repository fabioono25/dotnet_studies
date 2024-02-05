using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models;
public class Student
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }

    [DataType(DataType.Date)]
    public DateTime? InputDate { get; set; }

    public bool? Active { get; set; }
}
