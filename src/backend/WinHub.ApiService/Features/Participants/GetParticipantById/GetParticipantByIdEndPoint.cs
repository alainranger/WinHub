using Carter;

using MediatR;

namespace WinHub.ApiService.Features.Participants.GetParticipantById;

public class GetParticipantByIdEndPoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app) =>
		app.MapGet("api/participant/{id}", async (Guid id, ISender sender) =>
		{
			var query = new GetParticipantByIdQuery { Id = id };

			var result = await sender.Send(query).ConfigureAwait(true);

			if (result.IsFailure)
				return Results.NotFound(result.Error);

			return Results.Ok(result);
		});
}
