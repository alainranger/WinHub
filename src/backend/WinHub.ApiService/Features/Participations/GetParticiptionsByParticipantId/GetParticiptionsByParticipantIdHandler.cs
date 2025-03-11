
using Mapster;

using MediatR;

using Microsoft.EntityFrameworkCore;

using WinHub.ApiService.Common;
using WinHub.ApiService.Contracts.ParticipationFeature;
using WinHub.ApiService.Database;

namespace WinHub.ApiService.Features.Participations.GetParticiptionsByParticipantId;

public class GetParticiptionsByParticipantIdHandler(WinHubContext dbContest) : IRequestHandler<GetParticiptionsByParticipantIdQuery, Result<List<ParticipationResponse>>>
{
	public async Task<Result<List<ParticipationResponse>>> Handle(GetParticiptionsByParticipantIdQuery request, CancellationToken cancellationToken)
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
