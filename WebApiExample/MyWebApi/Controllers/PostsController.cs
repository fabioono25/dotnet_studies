using Microsoft.AspNetCore.Mvc;
using MyWebApi.Models;
using MyWebApi.Services;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController(IPostService postService) : ControllerBase
    {
        // private readonly IPostService _postsService;
        // public PostsController(IPostService postService)
        // {
        //     _postsService = postService;
        // }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            // var post = await _postsService.GetPost(id);
            var post = await postService.GetPost(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }
        
        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost(Post post)
        {
            await postService.CreatePost(post);
            return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePost(int id, Post post)
        {
            if (id != post.Id)
            {
                return BadRequest();
            }
            var updatedPost = await postService.UpdatePost(id, post);
            if (updatedPost == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

    }
}
