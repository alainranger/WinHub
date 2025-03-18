using FluentValidation;
using MediatR;
using WinHub.ApiService.Database;
using WinHub.Shared.Common;

namespace WinHub.ApiService.Features.Contests.UpdateContest;

public class UpdateContestHandler(WinHubContext dbContext, IValidator<UpdateContestCommand> validator) : IRequestHandler<UpdateContestCommand, Result>
{
	public async Task<Result> Handle(UpdateContestCommand request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		var validatorResult = await validator.ValidateAsync(request, cancellationToken).ConfigureAwait(false);
		if (!validatorResult.IsValid)
			return Result.Failure<Guid>(new ApiError(
				"UpdateContest.Validation",
				validatorResult.ToString()
			));

		var contest = await dbContext
				.Contests.FindAsync([request.Id], cancellationToken)
				.ConfigureAwait(true);

		if (contest == null)
			return Result.Failure(new ApiError(
				"UpdateContest.Null",
				"The contest with the specified ID was not found"
			));

		contest.Name = request.Name;
		contest.Description = request.Description;
		contest.StartDateTime = request.StartDateTime;
		contest.EndDateTime = request.EndDateTime;
		contest.ContestDateTime = contest.StartDateTime;

		dbContext.Update(contest);

		await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

		return Result.Success(contest);
	}
}
