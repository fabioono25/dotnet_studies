using DemoWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {

        [HttpGet(Name = "students")]
        public IEnumerable<Student> Get()
        {
            return new[] { new Student { Id=1, Name = "Jane"}, new Student { Id=2, Name = "John"} };
        }
    }
}
