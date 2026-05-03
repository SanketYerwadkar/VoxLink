using FluentValidation;
using MediatR;

namespace VoxLink.API.Application.Features.User.CreateUser;

public record CreateUserCommand(string Email, string Name, string Password) : IRequest<Result<CreateUserResponse>>;

public record CreateUserResponse(Guid Id, string Email, string Name, DateTime CreatedAt);

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Valid email is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100)
            .WithMessage("Name must be between 2 and 100 characters");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]")
            .WithMessage("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character");
    }
}
