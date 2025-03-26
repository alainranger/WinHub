using System.Text.Json;
using System.Text.Json.Serialization;
using Mapster;
using WinHub.Blazor.Models;
using WinHub.Shared.Contracts.ContestFeature;

namespace WinHub.Blazor.Services;

internal class ContestService(HttpClient httpClient) : IContestService
{
	private readonly HttpClient _httpClient = httpClient;

	public async Task<List<Contest>> GetContestsAsync(CancellationToken cancellationToken = default)
	{
		var response = await _httpClient.GetAsync(new Uri("api/contests", UriKind.Relative), cancellationToken).ConfigureAwait(true);

		if (response.IsSuccessStatusCode)
		{
			var contestsReponse = await response.Content
				.ReadFromJsonAsync<List<ContestResponse>>(cancellationToken)
				.ConfigureAwait(true) ?? throw new InvalidOperationException("Failed to deserialize contests.");
			return contestsReponse
				.Adapt<List<Contest>>();
		}
		else
		{
			return [];
		}
	}

	public async Task<Contest?> GetContestAsync(Guid contestId, CancellationToken cancellationToken = default) => await _httpClient.GetFromJsonAsync<Contest>($"api/contests/{contestId}", cancellationToken).ConfigureAwait(true);

	public async Task<Guid> CreateContestAsync(Contest contest, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(contest);

		var response = await _httpClient.PostAsJsonAsync("api/contests", contest, cancellationToken).ConfigureAwait(true);
		if (!response.IsSuccessStatusCode)
		{
			throw new InvalidOperationException("Failed to deserialize contest.");
		}

		var newCcontest = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(true);
		if (Guid.TryParse(newCcontest, out var newConestGuid))
			return newConestGuid;
		else
			throw new InvalidOperationException("Failed to parse GUID");
	}

	public async Task<bool> UpdateContestAsync(Contest contest, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(contest);

		var response = await _httpClient.PutAsJsonAsync($"api/contests/{contest.Id}", contest, cancellationToken).ConfigureAwait(true);
		if (!response.IsSuccessStatusCode)
		{
			return false;
		}

		return true;
	}

	public async Task DeleteContestAsync(Guid contestId, CancellationToken cancellationToken = default) => await _httpClient.DeleteAsync(new Uri($"api/contests/{contestId}"), cancellationToken).ConfigureAwait(true);
}


// public class ConstestReponse
// {
// 	[JsonPropertyName("id")]
// 	public Guid Id { get; set; }

// 	[JsonPropertyName("name")]
// 	public string Name { get; set; }

// 	[JsonPropertyName("description")]
// 	public string Description { get; set; }

// 	[JsonPropertyName("startDateTime")]
// 	public DateTimeOffset StartDateTime { get; set; }

// 	[JsonPropertyName("endDateTime")]
// 	public DateTimeOffset EndDateTime { get; set; }

// 	[JsonPropertyName("contestDateTime")]
// 	public DateTimeOffset ContestDateTime { get; set; }
// }
