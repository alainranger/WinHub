using FluentValidation;

using MediatR;

using WinHub.ApiService.Common;
using WinHub.ApiService.Database;
using WinHub.ApiService.Entities;

namespace WinHub.ApiService.Features.Contests.CreateContest;

public class CreateContestHandler(WinHubContext dbContext, IValidator<CreateContestCommand> validator) : IRequestHandler<CreateContestCommand, Result<Guid>>
{
	public async Task<Result<Guid>> Handle(CreateContestCommand request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		var validatorResult = await validator.ValidateAsync(request, cancellationToken).ConfigureAwait(false);
		if (!validatorResult.IsValid)
			return Result.Failure<Guid>(new ApiError(
				"CreateContest.Validation",
				validatorResult.ToString()
			));

		var contest = new Contest()
		{
			Id = Guid.NewGuid(),
			Name = request.Name,
			Description = request.Description,
			StartDateTime = request.StartDateTime,
			EndDateTime = request.EndDateTime,
			ContestDateTime = request.ContestDateTime
		};

		dbContext.Add(contest);

		await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

		return contest.Id;
	}
}
