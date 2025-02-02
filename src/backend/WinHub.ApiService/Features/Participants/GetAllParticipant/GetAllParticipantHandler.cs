
using Mapster;

using MediatR;

using Microsoft.EntityFrameworkCore;

using WinHub.ApiService.Common;
using WinHub.ApiService.Contracts.ParticipantFeature;
using WinHub.ApiService.Database;

namespace WinHub.ApiService.Features.Participants.GetAllParticipant;

public class GetAllParticipantHandler(WinHubContext dbContest) : IRequestHandler<GetAllParticipantQuery, Result<List<ParticipantResponse>>>
{
	public async Task<Result<List<ParticipantResponse>>> Handle(GetAllParticipantQuery request, CancellationToken cancellationToken)
	{
		var participantResponse = await dbContest.Participants
			.ProjectToType<ParticipantResponse>()
			.ToListAsync(cancellationToken)
			.ConfigureAwait(true);

		return participantResponse;
	}
}
