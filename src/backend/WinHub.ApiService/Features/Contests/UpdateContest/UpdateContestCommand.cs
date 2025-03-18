using MediatR;

using WinHub.Shared.Common;

namespace WinHub.ApiService.Features.Contests.UpdateContest;
public record UpdateContestCommand : IRequest<Result>
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public DateTime StartDateTime { get; set; }
	public DateTime EndDateTime { get; set; }
	public DateTime ContestDateTime { get; set; }
}
