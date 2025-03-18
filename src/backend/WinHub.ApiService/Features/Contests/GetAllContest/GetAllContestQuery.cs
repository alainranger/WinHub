using MediatR;

using WinHub.Shared.Common;
using WinHub.Shared.Contracts.ContestFeature;

namespace WinHub.ApiService.Features.Contests.GetAllContest;

public class GetAllContestsQuery : IRequest<Result<List<ContestResponse>>>
{
}
