using MediatR;

using WinHub.ApiService.Common;
using WinHub.ApiService.Database;

namespace WinHub.ApiService.Features.Contests.DeleteContest;

public class DeleteContestHandler(WinHubContext dbContext) : IRequestHandler<DeleteContestCommand, Result>
{
	public async Task<Result> Handle(DeleteContestCommand request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		var contest = await dbContext
			.Contests.FindAsync([request.Id], cancellationToken)
			.ConfigureAwait(true);

		if (contest == null)
			return Result.Failure(new ApiError(
				"DeleteContest.Null",
				"The contest with the specified ID was not found"
			));

		dbContext.Remove(contest);

		await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

		return Result.Success(contest);
	}
}
