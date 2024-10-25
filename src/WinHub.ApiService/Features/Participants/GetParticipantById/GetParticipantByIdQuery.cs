using MediatR;

using WinHub.ApiService.Common;
using WinHub.ApiService.Contracts.ParticipantFeature;

namespace WinHub.ApiService.Features.Participants.GetParticipantById;

public class GetParticipantByIdQuery : IRequest<Result<ParticipantResponse>>
{
	public Guid Id { get; set; }
}
