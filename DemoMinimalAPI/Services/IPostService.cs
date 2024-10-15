using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoMinimalAPI.Services
{
    public interface IPostService
    {
        Task<string> GetPost(int id);
    }
}