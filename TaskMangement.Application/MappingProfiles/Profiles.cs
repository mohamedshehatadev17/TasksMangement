
using Mapster;
using TaskMangement.Application.DTOs;
using TaskMangement.Application.Features.Tasks.Commands.CreateTask;
using TaskMangement.Domain.Models;

namespace TaskMangement.Application.configurations;
public static class MappingConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<CreateTaskCommand, Domain.Models.Task>
            .NewConfig()
            .Ignore(dest => dest.Id);

        TypeAdapterConfig<User, UserProfileResponse>
            .NewConfig();


        TypeAdapterConfig<Domain.Models.Task, TaskResponse>
            .NewConfig();
    }

}

