using DemoWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DemoWebAPI.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        public bool ValidOperation()
        {
            return true;
        }

        // create a customized actionresult
        protected ActionResult CustomResponse(object result = null)
        {
            if (ValidOperation())
            {
                return Ok(new { success = true, data = result });
            }

            //return BadRequest(new ValidationProblemDetails(new ModelStateDictionary(ModelState)));
            return BadRequest(new
            {
                success = false,
                errors = new[] { GetErrorMessages() }
            }); 
        }
        
        protected string GetErrorMessages()
        {
            return "An error occurred";
        }
    }   

    [ApiController]
    [Route("[controller]")]
    public class StudentController : MainController
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
                return CustomResponse(new Student { Id = id, Name = "Jane" });
                //return new Student { Id = id, Name = "Jane" };
            }
            else
            {
                //return NotFound();
                return CustomResponse();
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
