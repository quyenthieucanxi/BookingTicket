
using Application.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace Application.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfigureMediatR(this IServiceCollection services)
    {
        services.AddMediatR(typeof(Application.AssemblyReference).Assembly)
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
            .AddValidatorsFromAssembly(typeof(Application.AssemblyReference).Assembly,includeInternalTypes:true);
        return services;
    }
}