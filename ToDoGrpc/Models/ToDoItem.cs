namespace ToDoGrpc.Models;

public record ToDoItem
{
    public int Id { get; set; } // DB PK
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string ToDoStatus { get; set; } = "NEW";
}