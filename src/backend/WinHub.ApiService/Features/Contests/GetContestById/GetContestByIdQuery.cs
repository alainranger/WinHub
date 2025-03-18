using MediatR;

using WinHub.Shared.Common;
using WinHub.Shared.Contracts.ContestFeature;

namespace WinHub.ApiService.Features.Contests.GetContestById;

public class GetContestByIdQuery : IRequest<Result<ContestResponse>>
{
	public Guid Id { get; set; }
}
