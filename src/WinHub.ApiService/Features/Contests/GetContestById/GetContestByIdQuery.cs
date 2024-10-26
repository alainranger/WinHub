using MediatR;

using WinHub.ApiService.Common;
using WinHub.ApiService.Contracts.ContestFeature;

namespace WinHub.ApiService.Features.Contests.GetContestById;

public class GetContestByIdQuery : IRequest<Result<ContestResponse>>
{
	public Guid Id { get; set; }
}
