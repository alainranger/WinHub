
using Mapster;

using MediatR;

using Microsoft.EntityFrameworkCore;

using WinHub.ApiService.Common;
using WinHub.ApiService.Contracts.ContestFeature;
using WinHub.ApiService.Database;

namespace WinHub.ApiService.Features.Contests.GetAllContest;

public class GetAllContestHandler(WinHubContext dbContest) : IRequestHandler<GetAllContestsQuery, Result<List<ContestResponse>>>
{
	public async Task<Result<List<ContestResponse>>> Handle(GetAllContestsQuery request, CancellationToken cancellationToken)
	{
		var contestsResponse = await dbContest.Contests
			.ProjectToType<ContestResponse>()
			.ToListAsync(cancellationToken)
			.ConfigureAwait(true);

		return contestsResponse;
	}
}
