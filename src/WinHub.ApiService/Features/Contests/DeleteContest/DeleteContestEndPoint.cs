using Carter;

using MediatR;

namespace WinHub.ApiService.Features.Contests.DeleteContest;

public class DeleteContestEndPoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app) =>
		app.MapDelete("api/contest", async (Guid id, ISender sender) =>
		{
			var command = new DeleteContestCommand { Id = id };

			var result = await sender.Send(command).ConfigureAwait(false);

			if (result.IsFailure)
				return Results.BadRequest(result.Error);

			return Results.Ok(result.Value);
		});
}
