using Carter;

using MediatR;

namespace WinHub.ApiService.Features.Participants.GetAllParticipant;

public class GetAllParticipantEndPoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app) =>
		app.MapGet("api/participants", async (ISender sender) =>
		{
			var query = new GetAllParticipantQuery { };

			var result = await sender.Send(query).ConfigureAwait(true);

			if (result.IsFailure)
				return Results.NotFound(result.Error);

			return Results.Ok(result);
		});
}
