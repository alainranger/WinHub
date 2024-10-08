
using MediatR;

using Microsoft.EntityFrameworkCore;

using WinHub.ApiService.Common;
using WinHub.ApiService.Contracts;
using WinHub.ApiService.Database;

namespace WinHub.ApiService.Features.Contests.GetContestById;

public class GetContestByIdHandler(WinHubContext dbContest) : IRequestHandler<GetContestByIdQuery, Result<ContestResponse>>
{
	public async Task<Result<ContestResponse>> Handle(GetContestByIdQuery request, CancellationToken cancellationToken)
	{
		var contestsResponse = await dbContest
			.Contests
			.Where(contest => contest.Id == request.Id)
			.Select(contest => new ContestResponse
			{
				Id = contest.Id,
				Name = contest.Name,
				Description = contest.Description,
				StartDateTime = contest.StartDateTime,
				EndDateTime = contest.EndDateTime,
				ContestDateTime = contest.ContestDateTime,
			})
			.FirstOrDefaultAsync(cancellationToken)
			.ConfigureAwait(true);

		if (contestsResponse == null)
			return Result.Failure<ContestResponse>(new ApiError(
				"GetContestById.Null",
				"The contest with the specified ID was not found"
			));

		return contestsResponse;
	}
}
