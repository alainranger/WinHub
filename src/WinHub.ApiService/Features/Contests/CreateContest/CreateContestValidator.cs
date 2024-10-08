using FluentValidation;

namespace WinHub.ApiService.Features.Contests.CreateContest;

public class UpdateContestValidator : AbstractValidator<UpdateContestCommand>
{
	public UpdateContestValidator()
	{
		RuleFor(c => c.Name).NotEmpty();
		RuleFor(c => c.Description).NotEmpty();
		RuleFor(c => c.StartDateTime).NotNull();
		RuleFor(c => c.EndDateTime).NotNull();
		RuleFor(c => c.ContestDateTime).NotNull();
	}
}
