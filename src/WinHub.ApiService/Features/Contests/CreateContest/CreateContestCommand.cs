using MediatR;

using WinHub.ApiService.Common;

namespace WinHub.ApiService.Features.Contests.CreateContest;

public record UpdateContestCommand : IRequest<Result<Guid>>
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public DateTime StartDateTime { get; set; }
	public DateTime EndDateTime { get; set; }
	public DateTime ContestDateTime { get; set; }
}
