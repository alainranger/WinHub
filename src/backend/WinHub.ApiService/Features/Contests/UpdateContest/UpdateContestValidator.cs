using FluentValidation;

namespace WinHub.ApiService.Features.Contests.UpdateContest;

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
