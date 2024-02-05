using DemoRazorPages.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DemoRazorPages.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly DemoRazorPages.Data.ApplicationDbContext _context;

        public IndexModel(DemoRazorPages.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Student> Student { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Student != null)
            {
                Student = await _context.Student.ToListAsync();
            }
        }
    }
}
