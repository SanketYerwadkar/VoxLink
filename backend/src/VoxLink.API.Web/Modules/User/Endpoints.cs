using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VoxLink.API.Application.Features.User.CreateUser;

namespace VoxLink.API.Web.Modules.User;

public class Endpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users");

        group.MapPost("/", CreateUser)
            .WithName("CreateUser")
            .Produces<CreateUserResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create a new user")
            .WithDescription("Creates a new user account with email, name, and password");
    }

    private static async Task<IResult> CreateUser([FromBody] CreateUserCommand command, ISender sender)
    {
        var result = await sender.Send(command);

        if (result.IsFailure)
        {
            return Results.BadRequest(result.Error);
        }

        return Results.Created($"/api/users/{result.Value.Id}", result.Value);
    }
}
