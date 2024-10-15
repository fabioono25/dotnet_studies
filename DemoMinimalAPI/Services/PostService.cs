using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoMinimalAPI.Services
{
    public class PostService : IPostService
    {
        public Task<string> GetPost(int id)
        {
            return Task.FromResult("this is a post");
        }
    }
}