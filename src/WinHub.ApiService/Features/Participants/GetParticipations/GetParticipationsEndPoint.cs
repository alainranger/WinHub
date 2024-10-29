using Carter;

using MediatR;

namespace WinHub.ApiService.Features.Participants.GetParticipations;

public class GetParticipationsByContestIdEndPoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app) =>
		app.MapGet("api/participant/{id}/participations", async (Guid id, ISender sender) =>
		{
			var query = new GetParticipationsQuery { ParticipantId = id };

			var result = await sender.Send(query).ConfigureAwait(true);

			if (result.IsFailure)
				return Results.NotFound(result.Error);

			return Results.Ok(result);
		});
}
