using System.Net;
using Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.IdentityModel.Tokens;

namespace Presentation.Middleware;

public class AuthorizationMiddlewareHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();


    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (context.Items.TryGetValue("AuthError", out var authError))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            if (authError is string errorType)
            {
                var result = errorType switch
                {
                    "TokenExpired" => new { error = "TokenExpired", message = "The token has expired." },
                    "InvalidToken" => new { error = "InvalidToken", message = "The token is invalid." },
                    "Unauthorized" => new { error = "Unauthorized", message = "You are not authorized to access this resource." },
                    _ => new { error = "Unauthorized", message = "Authorization failed." }
                };

                await context.Response.WriteAsJsonAsync(result);
                return;
            }
        }
        await defaultHandler.HandleAsync(next, context, policy, authorizeResult);

    }
}