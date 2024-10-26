using MediatR;

using WinHub.ApiService.Common;
using WinHub.ApiService.Contracts.ContestFeature;

namespace WinHub.ApiService.Features.Contests.GetAllContest;

public class GetAllContestsQuery : IRequest<Result<List<ContestResponse>>>
{
}
