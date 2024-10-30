using MediatR;

using WinHub.ApiService.Common;
using WinHub.ApiService.Contracts.ParticipationFeature;

namespace WinHub.ApiService.Features.Contests.GetParticipations;

public class GetParticipationsQuery : IRequest<Result<List<ParticipationResponse>>>
{
	public Guid ContestId { get; set; }
}
