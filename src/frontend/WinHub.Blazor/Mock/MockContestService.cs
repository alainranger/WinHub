using WinHub.Blazor.Models;
using WinHub.Blazor.Services;
using WinHub.Shared.Contracts.ContestFeature;

namespace WinHub.Blazor.Mock;

internal class MockContestService : IContestService
{
	private readonly List<Contest> _contests =
	[
		new Contest
		{
			Id = Guid.NewGuid(),
			Name = "Contest 1",
			Description = "Description for Contest 1",
			StartDateTime = DateTime.UtcNow,
			EndDateTime = DateTime.UtcNow.AddDays(7),
			ContestDateTime = DateTime.UtcNow.AddDays(8)
		},
		new Contest
		{
			Id = Guid.NewGuid(),
			Name = "Contest 2",
			Description = "Description for Contest 2",
			StartDateTime = DateTime.UtcNow.AddDays(-7),
			EndDateTime = DateTime.UtcNow.AddDays(7),
			ContestDateTime =  DateTime.UtcNow.AddDays(8)
		}
	];

	public Task<Guid> CreateContestAsync(Contest contest, CancellationToken cancellationToken = default)
	{
		contest.Id = Guid.NewGuid();
		_contests.Add(new Contest
		{
			Id = contest.Id,
			Name = contest.Name,
			Description = contest.Description,
			StartDateTime = contest.StartDateTime,
			EndDateTime = contest.EndDateTime,
			ContestDateTime = contest.ContestDateTime
		});
		return Task.FromResult(contest.Id);
	}

	public Task<List<Contest>> GetContestsAsync(CancellationToken cancellationToken = default) => Task.FromResult(_contests);

	public Task<Contest?> GetContestAsync(Guid contestId, CancellationToken cancellationToken = default) => Task.FromResult(_contests.Find(x => x.Id == contestId));

	public Task DeleteContestAsync(Guid contestId, CancellationToken cancellationToken = default)
	{
		var contest = _contests.Find(x => x.Id == contestId);
		if (contest == null)
			return Task.FromResult(false);
		return Task.FromResult(_contests.Remove(contest));
	}

	public Task<bool> UpdateContestAsync(Contest contest, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(contest);

		var currentContest = _contests.Find(x => x.Id == contest.Id);

		if (currentContest == null)
			return Task.FromResult(false);

		currentContest.Name = contest.Name;
		currentContest.Description = contest.Description;
		currentContest.StartDateTime = contest.StartDateTime;
		currentContest.EndDateTime = contest.EndDateTime;
		currentContest.ContestDateTime = contest.ContestDateTime;

		return Task.FromResult(true);
	}
}
