using MediatR;
using WinHub.ApiService.Contracts.ParticipationFeature;
using WinHub.Shared.Common;

namespace WinHub.ApiService.Features.Contests.GetParticipations;

public class GetParticipationsQuery : IRequest<Result<List<ParticipationResponse>>>
{
	public Guid ContestId { get; set; }
}
