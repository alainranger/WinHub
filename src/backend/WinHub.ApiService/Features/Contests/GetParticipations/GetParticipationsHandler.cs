
using Mapster;

using MediatR;
using Microsoft.EntityFrameworkCore;
using WinHub.ApiService.Contracts.ParticipationFeature;
using WinHub.ApiService.Database;
using WinHub.Shared.Common;

namespace WinHub.ApiService.Features.Contests.GetParticipations;

public class GetParticipationsHandler(WinHubContext dbContest) : IRequestHandler<GetParticipationsQuery, Result<List<ParticipationResponse>>>
{
	public async Task<Result<List<ParticipationResponse>>> Handle(GetParticipationsQuery request, CancellationToken cancellationToken)
	{
		var participationResponse = await dbContest
			.Participations
			.Where(participation => participation.ContestId == request.ContestId)
			.ProjectToType<ParticipationResponse>()
			.ToListAsync(cancellationToken)
			.ConfigureAwait(true);

		return participationResponse;
	}
}
