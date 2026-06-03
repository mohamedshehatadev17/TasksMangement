using System.Reflection;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TaskMangement.Application.Abstractions.Authentication;
using TaskMangement.Application.configurations;
namespace Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddSingleton(TypeAdapterConfig.GlobalSettings);
            services.AddScoped<IMapper, ServiceMapper>();
            MappingConfig.RegisterMappings();
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
