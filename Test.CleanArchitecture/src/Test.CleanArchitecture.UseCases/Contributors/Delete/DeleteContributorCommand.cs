using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Test.CleanArchitecture.UseCases.Contributors.Delete;

public record DeleteContributorCommand(int ContributorId) : ICommand<Result>;
