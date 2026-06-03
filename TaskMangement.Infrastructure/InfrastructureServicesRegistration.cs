using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using TaskMangement.Application.Abstractions.Authentication;
using TaskMangement.Application.Abstractions.Contracts;
using TaskMangement.Application.Abstractions.Contracts.Persistance;
using TaskMangement.Domain.Models;
using TaskMangement.Infrastructure.Authentication;
using TaskMangement.Infrastructure.Persistance.Authentication;
using TaskMangement.Infrastructure.Persistance.Contexts;
using TaskMangement.Infrastructure.Queue;
using TaskMangement.Infrastructure.Redis;
using TaskMangement.Infrastructure.Repos;
using TaskMangement.Infrastructure.Workers;

namespace Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddSingleton<IBackgroundTaskQueue,BackgroundTaskQueue>();
        services.AddHostedService<TaskProcessingService>();







        return services;
    }
}