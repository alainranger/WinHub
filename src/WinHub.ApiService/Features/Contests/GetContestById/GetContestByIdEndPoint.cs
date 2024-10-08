using Carter;

using MediatR;

namespace WinHub.ApiService.Features.Contests.GetContestById;

public class GetContestByIdEndPoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app) =>
		app.MapGet("api/contest/{id}", async (Guid id, ISender sender) =>
		{
			var query = new GetContestByIdQuery { Id = id };

			var result = await sender.Send(query).ConfigureAwait(true);

			if (result.IsFailure)
				return Results.NotFound(result.Error);

			return Results.Ok(result);
		});
}
