using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApi.Services;

public interface IService
{
    string Name { get; }
    string SayHello();
}