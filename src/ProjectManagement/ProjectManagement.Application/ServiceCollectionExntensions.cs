using FluentValidation;
//using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectManagement.Application;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining(typeof(ServiceCollectionExtensions));

        // services.AddMediator(
        //     (MediatorOptions options) =>
        //     {
        //         options.Assemblies = [typeof(ServiceCollectionExtensions)];
        //         options.ServiceLifetime = ServiceLifetime.Scoped;
        //     }
        // );
    }
}
