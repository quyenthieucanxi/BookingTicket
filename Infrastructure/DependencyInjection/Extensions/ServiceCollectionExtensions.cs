using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Caching;
using Infrastructure.MessageBroker;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ICacheService, RedisCacheService>();
        services.AddTransient<IEventBus, EventBus>();
        services.Scan(scan => scan
            .FromAssembliesOf(typeof(Repository<>))  
            .AddClasses(classes => classes.InNamespaces("Infrastructure.Repositories")) 
            .AsImplementedInterfaces()
            .WithScopedLifetime());   

        services.AddScoped<IEmailService, EmailService>();
        return services;
    }
    
    
    
}