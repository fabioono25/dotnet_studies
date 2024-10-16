﻿using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Test.CleanArchitecture.UseCases.Contributors.Get;

public record GetContributorQuery(int ContributorId) : IQuery<Result<ContributorDTO>>;
