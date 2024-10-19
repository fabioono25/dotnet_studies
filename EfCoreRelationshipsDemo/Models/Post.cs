namespace EfCoreRelationshipsDemo.Models;

public class Post
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid? CategoryId { get; set; } // nullable - why: to allow the post to be created without a category
    public Category? Category { get; set; } // nullable - why: to allow the post to be created without a category
}
