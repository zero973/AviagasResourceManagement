using ARM.Core.Commands.Handlers.Entities;
using ARM.DAL.Repositories;

namespace ARM.WebApi.Extensions;

public static class ServiceCollectionExtensions
{

    /// <summary>
    /// Зарегистрировать все БД-репозитории вида <see cref="Core.Repositories.IDbEntitiesRepository"/>.
    /// </summary>
    public static void RegisterEntityRepositoriesFromAssembly(this IServiceCollection services)
    {
        // регистрируем BaseDbEntitiesRepository
        var baseRepoType = typeof(BaseDbEntitiesRepository<,>);
        var assembly = baseRepoType.Assembly;

        var repositories = assembly.GetTypes()
            .Where(x => !x.IsAbstract && x.BaseType!.GUID.Equals(baseRepoType.GUID)).ToList();

        foreach (var repository in repositories)
            services.AddTransient(repository.GetInterfaces()[0], repository);
        
        // регистрируем BaseDbActualEntitiesRepository
        baseRepoType = typeof(BaseDbActualEntitiesRepository<,>);
        
        repositories = assembly.GetTypes()
            .Where(x => !x.IsAbstract && x.BaseType!.GUID.Equals(baseRepoType.GUID)).ToList();

        foreach (var repository in repositories)
            services.AddTransient(repository.GetInterfaces()[0], repository);
    }

    /// <summary>
    /// Регистрация MediatR - <see cref="IRequest"/> и <see cref="IRequestHandler"/>.
    /// </summary>
    public static void RegisterMediatR(this IServiceCollection services)
    {
        var assembly = typeof(GetDataHandler<>).Assembly;

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(assembly);
            cfg.RegisterGenericHandlers = true;
        });
    }

}