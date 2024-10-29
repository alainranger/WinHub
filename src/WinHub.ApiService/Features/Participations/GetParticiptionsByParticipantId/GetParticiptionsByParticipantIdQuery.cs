using MediatR;

using WinHub.ApiService.Common;
using WinHub.ApiService.Contracts.ParticipationFeature;

namespace WinHub.ApiService.Features.Participations.GetParticiptionsByParticipantId;

public class GetParticiptionsByParticipantIdQuery : IRequest<Result<List<ParticipationResponse>>>
{
	public Guid ParticipantId { get; set; }
}
