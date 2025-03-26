using WinHub.Blazor.Models;
using WinHub.Shared.Common;
using WinHub.Shared.Contracts.ContestFeature;

namespace WinHub.Blazor.Services;

internal interface IContestService
{
	Task<List<Contest>> GetContestsAsync(CancellationToken cancellationToken = default);
	Task<Contest?> GetContestAsync(Guid contestId, CancellationToken cancellationToken = default);
	Task<Guid> CreateContestAsync(Contest contest, CancellationToken cancellationToken = default);
	Task<bool> UpdateContestAsync(Contest contest, CancellationToken cancellationToken = default);
	Task DeleteContestAsync(Guid contestId, CancellationToken cancellationToken = default);
}
