using MediatR;

using WinHub.ApiService.Common;
using WinHub.ApiService.Contracts.ParticipationFeature;

namespace WinHub.ApiService.Features.Participants.GetParticipations;

public class GetParticipationsQuery : IRequest<Result<List<ParticipationResponse>>>
{
	public Guid ParticipantId { get; set; }
}
