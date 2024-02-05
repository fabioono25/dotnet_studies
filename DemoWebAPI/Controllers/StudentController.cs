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
    [ApiConventionType(typeof(DefaultApiConventions))] // apply default api conventions to all actions in this controller. It control the response status code
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
            if (id < 0)
            {
                return BadRequest();
            }

            if (id == 1)
            {
                //return CustomResponse(new Student { Id = id, Name = "Jane" });
                return new Student { Id = id, Name = "Jane" };
            }
            else
            {
                return NotFound();
                //return CustomResponse();
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Student), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType] // if no other response type is specified, this will be used
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
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        public ActionResult<Student> Put(int id, Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != student.Id)
            {
                return NotFound();
            }

            //if (id != student.Id)
            //{
            //    return BadRequest();
            //}
            //else
            //{
            //    return Ok(student);
            //}
            return Ok(student);
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
