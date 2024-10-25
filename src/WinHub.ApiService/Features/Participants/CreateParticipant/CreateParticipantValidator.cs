using FluentValidation;

namespace WinHub.ApiService.Features.Participants.CreateParticipant;

public class CreateParticipantValidator : AbstractValidator<CreateParticipantCommand>
{
        public CreateParticipantValidator()
        {
                RuleFor(c => c.Firstname).NotEmpty();
                RuleFor(c => c.Lastname).NotEmpty();
                RuleFor(c => c.Email).EmailAddress().NotEmpty();
        }
}