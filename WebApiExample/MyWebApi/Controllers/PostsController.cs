using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApi.Models;
using MyWebApi.Services;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly PostsService _postsService;
        public PostsController()
        {
            _postsService = new PostsService(); // TODO: Use dependency injection, because this is bad practice.
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _postsService.GetPost(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }
        
        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost(Post post)
        {
            await _postsService.CreatePost(post);
            return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePost(int id, Post post)
        {
            if (id != post.Id)
            {
                return BadRequest();
            }
            var updatedPost = await _postsService.UpdatePost(id, post);
            if (updatedPost == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

    }
}
