using Application.Abstractions;
using Application.Bookings.Events;
using Application.DependencyInjection.Extensions;
using Carter;
using Domain.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Authentication;
using Infrastructure.BackgroundJobs;
using Infrastructure.DependencyInjection.Extensions;
using Infrastructure.MessageBroker;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Data;
using Persistence.Interceptors;
using Presentation.Middleware;
using Presentation.OptionsSetup;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "BookingTicket", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
        });
    });
builder.Services.AddSingleton<AddOrUpdateAuditableEntitiesInterceptor>();
builder.Services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
{
    var outBoxMessagesInterceptor = sp.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>();
    var addOrUpdateInterceptor = sp.GetRequiredService<AddOrUpdateAuditableEntitiesInterceptor>();
    options.UseNpgsql(builder.Configuration.GetConnectionString("Application"))
        .AddInterceptors(addOrUpdateInterceptor!,outBoxMessagesInterceptor!);
}); 

builder.Services.AddStackExchangeRedisCache(redisOptions =>
{
    var connection = builder.Configuration.GetConnectionString("Redis");
    redisOptions.Configuration = connection;   
});

builder.Services.Configure<MessageBrokerSettings>(
    builder.Configuration.GetSection("MessageBroker"));

builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

builder.Services.AddMassTransit(busConfigurator =>
    {
        busConfigurator.SetDefaultEndpointNameFormatter();
        busConfigurator.AddConsumer<BookingRegisterDomainEventConsumer>();
        busConfigurator.UsingRabbitMq((context, configrator) =>
        {
            configrator.UseNewtonsoftJsonSerializer();  
            MessageBrokerSettings settings = context.GetRequiredService<MessageBrokerSettings>();
            configrator.Host(settings.Host,"/", h =>
            {
                h.Username(settings.UserName);
                h.Password(settings.Password);
            });
            configrator.ReceiveEndpoint("booking-register-queue", e =>
            {
                e.ConfigureConsumer<BookingRegisterDomainEventConsumer>(context); 
            });
        });
    }
);

builder.Services.AddCarter();

builder.Services.AddConfigureMediatR();

builder.Services.AddInfrastructure();

builder.Services.AddQuartz(configure =>
{
    var jobKey = new JobKey(nameof(ProcessOutboxMessageJob));
    configure.AddJob<ProcessOutboxMessageJob>(jobKey)
        .AddTrigger(
            trigger => trigger.ForJob(jobKey)
                .WithSimpleSchedule(
                    schedule => schedule.WithIntervalInSeconds(10)
                        .RepeatForever()
                )

        );
    configure.UseMicrosoftDependencyInjectionJobFactory();
});
builder.Services.AddTransient<IJwtProvider,JwtProvider>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddQuartzHostedService();

builder.Services.AddIdentity<User,IdentityRole<Guid>>(options =>
    {
        options.SignIn.RequireConfirmedEmail = true;
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.SignIn.RequireConfirmedEmail = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthorizationMiddlewareHandler>();
builder.Services.AddCors();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer();
builder.Services.AddAuthorization();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapCarter();


app.Run();