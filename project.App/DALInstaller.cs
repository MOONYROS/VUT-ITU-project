using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using project.App.Options;
using project.DAL.Factories;
using project.DAL;
using project.DAL.Mappers;
using CommunityToolkit.Maui;

namespace project.App;

public static class DALInstaller
{
	public static IServiceCollection AddDALServices(this IServiceCollection services, IConfiguration configuration)
	{
		DALOptions dalOptions = new();
		configuration.GetSection("project:DAL").Bind(dalOptions);

		services.AddSingleton(dalOptions);

        if (dalOptions.Sqlite is null)
        {
            throw new InvalidOperationException("No persistence provider configured");
        }

        if (dalOptions.Sqlite?.Enabled == true)
        {
            if (dalOptions.Sqlite.DatabaseName is null)
            {
                throw new InvalidOperationException($"{nameof(dalOptions.Sqlite.DatabaseName)} is not set");
            }
            string databaseFilePath = Path.Combine(Environment.CurrentDirectory, @"..\project.DAL\" , dalOptions.Sqlite.DatabaseName!);
            services.AddSingleton<IDbContextFactory<ProjectDbContext>>(provider => new DbContextSqLiteFactory(databaseFilePath));
            services.AddSingleton<IDbMigrator, SqliteDbMigrator>();
        }

        services.AddSingleton<ActivityEntityMapper>();
        services.AddSingleton<ActivityTagListEntityMapper>();
        services.AddSingleton<ProjectEntityMapper>();
        services.AddSingleton<TagEntityMapper>();
        services.AddSingleton<TodoEntityMapper>();
        services.AddSingleton<UserEntityMapper>();
        services.AddSingleton<UserProjectListEntityMapper>();

        return services;
	}
}
