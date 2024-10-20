using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo.Models;

[Keyless]
public class BlogPostsCount
{
    public string BlogName { get; set; }
    public int PostCount { get; set; }
}
