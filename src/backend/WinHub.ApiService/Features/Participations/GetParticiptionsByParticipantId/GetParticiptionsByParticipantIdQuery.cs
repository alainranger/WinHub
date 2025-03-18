using MediatR;
using WinHub.ApiService.Contracts.ParticipationFeature;
using WinHub.Shared.Common;

namespace WinHub.ApiService.Features.Participations.GetParticiptionsByParticipantId;

public class GetParticiptionsByParticipantIdQuery : IRequest<Result<List<ParticipationResponse>>>
{
	public Guid ParticipantId { get; set; }
}
