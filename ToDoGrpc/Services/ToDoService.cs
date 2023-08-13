using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ToDoGrpc.Data;
using ToDoGrpc.Models;

namespace ToDoGrpc.Services;

public class ToDoService : ToDoIt.ToDoItBase
{
  private AppDbContext _context;

  public ToDoService(AppDbContext context)
  {
    _context = context;
  }

  public override async Task<CreateToDoResponse> CreateToDo(CreateToDoRequest request, ServerCallContext context)
  {
    if (request.Title == String.Empty || request.Description == String.Empty)
    {
      throw new RpcException(new Status(StatusCode.InvalidArgument, "Title and Description cannot be empty"));
    }

    var toDoItem = new ToDoItem
    {
      Title = request.Title,
      Description = request.Description
    };

    await _context.AddAsync(toDoItem);
    await _context.SaveChangesAsync();

    return await Task.FromResult(new CreateToDoResponse
    {
      Id = toDoItem.Id
    });
  }

  public override async Task<ReadToDoResponse> ReadToDo(ReadToDoRequest request, ServerCallContext context)
  {
    if (request.Id <= 0)
    {
      throw new RpcException(new Status(StatusCode.InvalidArgument, "Id cannot be less than or equal to 0"));
    }

    var toDoItem = await _context.ToDoItems.FirstOrDefaultAsync(t => t.Id == request.Id) ?? throw new RpcException(new Status(StatusCode.NotFound, $"ToDoItem with id {request.Id} not found"));

    return await Task.FromResult(new ReadToDoResponse
    {
      Id = toDoItem.Id,
      Title = toDoItem.Title,
      Description = toDoItem.Description,
      ToDoStatus = toDoItem.ToDoStatus
    });
  }

  public override Task<GetAllResponse> ListToDo(GetAllRequest request, ServerCallContext context)
  {
    var response = new GetAllResponse();
    var toDoItems = _context.ToDoItems.ToListAsync();

    foreach (var toDoItem in toDoItems.Result)
    {
      response.ToDos.Add(new ReadToDoResponse
      {
        Id = toDoItem.Id,
        Title = toDoItem.Title,
        Description = toDoItem.Description,
        ToDoStatus = toDoItem.ToDoStatus
      });
    }

    return Task.FromResult(response);
  }

  public override async Task<UpdateToDoResponse> UpdateToDo(UpdateToDoRequest request, ServerCallContext context)
  {
    if (request.Id <= 0 || request.Title == String.Empty || request.Description == String.Empty)
    {
      throw new RpcException(new Status(StatusCode.InvalidArgument, "You must provide a valid Id, Title and Description"));
    }

    var toDoItem = await _context.ToDoItems.FirstOrDefaultAsync(t => t.Id == request.Id) ?? throw new RpcException(new Status(StatusCode.NotFound, $"ToDoItem with id {request.Id} not found"));

    toDoItem.Title = request.Title;
    toDoItem.Description = request.Description;
    toDoItem.ToDoStatus = request.ToDoStatus;

    await _context.SaveChangesAsync();

    return await Task.FromResult(new UpdateToDoResponse
    {
      Id = toDoItem.Id
    });
  }

  public override async Task<DeleteToDoResponse> DeleteToDo(DeleteToDoRequest request, ServerCallContext context)
  {
    if (request.Id <= 0)
    {
      throw new RpcException(new Status(StatusCode.InvalidArgument, "Id cannot be less than or equal to 0"));
    }

    var toDoItem = await _context.ToDoItems.FirstOrDefaultAsync(t => t.Id == request.Id) ?? throw new RpcException(new Status(StatusCode.NotFound, $"ToDoItem with id {request.Id} not found"));

    _context.Remove(toDoItem);
    await _context.SaveChangesAsync();

    return await Task.FromResult(new DeleteToDoResponse
    {
      Id = toDoItem.Id
    });
  }
}

