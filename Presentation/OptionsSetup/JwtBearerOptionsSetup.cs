using System.Text;
using Domain.Shared;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Presentation.OptionsSetup;

public class JwtBearerOptionsSetup : IPostConfigureOptions<JwtBearerOptions>
{
    private readonly JwtOptions _jwtOptions;

    public JwtBearerOptionsSetup(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public void PostConfigure(string? name, JwtBearerOptions options)
    {
        options.TokenValidationParameters.ValidIssuer = _jwtOptions.Issuer;
        options.TokenValidationParameters.ValidAudience = _jwtOptions.Audience;
        options.TokenValidationParameters.IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        options.TokenValidationParameters.ValidateIssuer = true;
        options.TokenValidationParameters.ValidateAudience = true; 
        options.TokenValidationParameters.ValidateIssuerSigningKey = true;
        options.MapInboundClaims = false;
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("OnAuthenticationFailed");
                if (context.Exception is SecurityTokenExpiredException)
                {
                    // Set a flag to indicate token expiration
                    context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.HttpContext.Response.ContentType = "application/json";
                    context.HttpContext.Response.WriteAsJsonAsync(Result.Failure(new Error("TokenExpired", "The token has expired.")));
                }
                else
                {
                    // Set a flag for general token validation failure
                    context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.HttpContext.Response.ContentType = "application/json";
                    context.HttpContext.Response.WriteAsJsonAsync(Result.Failure(new Error("InvalidToken", "The token is invalid.")));
                }
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                context.HandleResponse(); // Prevents the default challenge response
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.WriteAsJsonAsync(Result.Failure(new Error("Unauthorized", "You are not authorized to access this resource.")));
                return Task.CompletedTask;
            }
        };

    }
}