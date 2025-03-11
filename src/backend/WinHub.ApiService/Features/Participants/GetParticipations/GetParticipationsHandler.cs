
using Mapster;

using MediatR;

using Microsoft.EntityFrameworkCore;

using WinHub.ApiService.Common;
using WinHub.ApiService.Contracts.ParticipationFeature;
using WinHub.ApiService.Database;

namespace WinHub.ApiService.Features.Participants.GetParticipations;

public class GetParticipationsHandler(WinHubContext dbContest) : IRequestHandler<GetParticipationsQuery, Result<List<ParticipationResponse>>>
{
	public async Task<Result<List<ParticipationResponse>>> Handle(GetParticipationsQuery request, CancellationToken cancellationToken)
	{
		var participationResponse = await dbContest
			.Participations
			.Where(participation => participation.ParticipantId == request.ParticipantId)
			.ProjectToType<ParticipationResponse>()
			.ToListAsync(cancellationToken)
			.ConfigureAwait(true);

		return participationResponse;
	}
}
