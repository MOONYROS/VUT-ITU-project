using Microsoft.Extensions.DependencyInjection;
using WpfApp1.BL.Facades;
using WpfApp1.BL.Facades.Interfaces;
using WpfApp1.BL.Mappers.Interfaces;

namespace WpfApp1.BL;

public static class BLInstaller
{
    public static IServiceCollection AddBLServices(this IServiceCollection services)
    {
	    /* //zakomentovano aby to nehazelo error, oprav to koumi pls
        services.AddSingleton<IUserProjectFacade, UserProjectFacade>();
        services.Scan(selector => selector
            .FromAssemblyOf<BusinessLogic>()
            .AddClasses(filter => filter.AssignableTo(typeof(IFacade<,,>)))
            .AsMatchingInterface()
            .WithSingletonLifetime()
        );
        */

        services.Scan(selector => selector
            .FromAssemblyOf<BusinessLogic>()
            .AddClasses(filter => filter.AssignableTo(typeof(IFacadeDetailOnly<,>)))
            .AsMatchingInterface()
            .WithSingletonLifetime()
        );

        services.Scan(selector => selector
            .FromAssemblyOf<BusinessLogic>()
            .AddClasses(filter => filter.AssignableTo(typeof(IModelMapper<,,>)))
            .AsMatchingInterface()
            .WithSingletonLifetime()
        );

        services.Scan(selector => selector
            .FromAssemblyOf<BusinessLogic>()
            .AddClasses(filter => filter.AssignableTo(typeof(IModelMapperDetailOnly<,>)))
            .AsMatchingInterface()
            .WithSingletonLifetime()
        );

        return services;
    }
}
