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
            return new[] { new Student { Id = 1, Name = "Jane" }, new Student { Id = 2, Name = "John" } };
        }

        //[HttpGet("{id}", Name = "student")]
        //public Student Get(int id)
        //{
        //    return new Student { Id = id, Name = "Jane" };
        //}

        // another get, using ActionResult
        // why ActionResult: because it can return NotFound() or Ok() or other status codes
        [HttpGet("{id}", Name = "student")]
        public ActionResult<Student> Get(int id)
        {
            if (id == 1)
            {
                return new Student { Id = id, Name = "Jane" };
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Student> Post(Student student)
        {
            if (student.Id == 1)
            {
                return BadRequest();
            }
            else
            {
                return CreatedAtRoute("student", new { id = student.Id }, student);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Student> Put(int id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }
            else
            {
                return Ok(student);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (id == 1)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
