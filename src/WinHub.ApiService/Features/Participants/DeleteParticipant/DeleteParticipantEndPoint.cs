using Carter;

using MediatR;

namespace WinHub.ApiService.Features.Participants.DeleteParticipant;

public class DeleteParticipantEndPoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app) =>
		app.MapDelete("api/participant", async (Guid id, ISender sender) =>
		{
			var command = new DeleteParticipantCommand { Id = id };

			var result = await sender.Send(command).ConfigureAwait(false);

			if (result.IsFailure)
				return Results.BadRequest(result.Error);

			return Results.Ok(result);
		});
}
