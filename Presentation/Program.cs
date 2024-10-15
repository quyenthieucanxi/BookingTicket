using Application.Abstractions;
using Application.DependencyInjection.Extensions;
using Carter;
using Domain.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Authentication;
using Infrastructure.BackgroundJobs;
using Infrastructure.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Data;
using Persistence.Interceptors;
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
builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
{
    var interceptor = sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>();
    options.UseNpgsql(builder.Configuration.GetConnectionString("Application"))
        .AddInterceptors(interceptor!);
});

builder.Services.AddStackExchangeRedisCache(redisOptions =>
{
    var connection = builder.Configuration.GetConnectionString("Redis");
    redisOptions.Configuration = connection;    
});

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
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();
builder.Services.AddAuthorization();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapCarter();
app.UseAuthentication();
app.UseAuthorization();


app.Run();