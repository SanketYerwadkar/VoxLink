using MediatR;
using Microsoft.AspNetCore.Identity;

namespace VoxLink.API.Application.Features.User.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<CreateUserResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public CreateUserCommandHandler(IApplicationDbContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<CreateUserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Check if user with same email already exists
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (existingUser != null)
        {
            return Result<CreateUserResponse>.Failure("User with this email already exists");
        }

        // Create new user
        var user = Domain.Entities.User.Create(request.Email, request.Name);
        
        // Hash password
        var passwordHash = _passwordHasher.HashPassword(user, request.Password);
        user.SetPassword(passwordHash);

        // Save to database
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        var response = new CreateUserResponse(user.Id, user.Email, user.Name, user.CreatedAt);
        return Result<CreateUserResponse>.Success(response);
    }
}
