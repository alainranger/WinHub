
using MediatR;

using Microsoft.EntityFrameworkCore;

using WinHub.ApiService.Common;
using WinHub.ApiService.Contracts.ParticipantFeature;
using WinHub.ApiService.Database;

namespace WinHub.ApiService.Features.Participants.GetParticipantById;

public class GetParticipantByIdHandler(WinHubContext dbContest) : IRequestHandler<GetParticipantByIdQuery, Result<ParticipantResponse>>
{
	public async Task<Result<ParticipantResponse>> Handle(GetParticipantByIdQuery request, CancellationToken cancellationToken)
	{
		var participantResponse = await dbContest
			.Participants
			.Where(participant => participant.Id == request.Id)
			.Select(participant => new ParticipantResponse
			{
				Id = participant.Id,
				Firstname = participant.Firstname,
				Lastname = participant.Lastname,
				Email = participant.Email,
			})
			.FirstOrDefaultAsync(cancellationToken)
			.ConfigureAwait(true);

		if (participantResponse == null)
			return Result.Failure<ParticipantResponse>(new ApiError(
				"GetParticipantById.Null",
				"The participant with the specified ID was not found"
			));

		return participantResponse;
	}
}
