using Carter;

using MediatR;

namespace WinHub.ApiService.Features.Contests.GetAllContest;

public class GetContestByIdEndPoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app) =>
		app.MapGet("api/contests", async (ISender sender) =>
		{
			var query = new GetAllContestsQuery { };

			var result = await sender.Send(query).ConfigureAwait(true);

			if (result.IsFailure)
				return Results.NotFound(result.Error);

			return Results.Ok(result);
		});
}
