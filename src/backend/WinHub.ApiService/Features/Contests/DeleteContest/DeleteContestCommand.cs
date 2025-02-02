using MediatR;

using WinHub.ApiService.Common;

namespace WinHub.ApiService.Features.Contests.DeleteContest;

public record DeleteContestCommand : IRequest<Result>
{
	public Guid Id { get; set; }
}
