using MediatR;
using WinHub.ApiService.Contracts.ParticipantFeature;
using WinHub.Shared.Common;

namespace WinHub.ApiService.Features.Participants.GetParticipantById;

public class GetParticipantByIdQuery : IRequest<Result<ParticipantResponse>>
{
	public Guid Id { get; set; }
}
