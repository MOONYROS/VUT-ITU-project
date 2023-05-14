﻿using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using project.DAL.Factories;
using project.DAL;
using project.DAL.Mappers;

namespace project.App;

public static class DALInstaller
{
	public static IServiceCollection AddDALServices(this IServiceCollection services, IConfiguration configuration)
	{
		string databaseFilePath = Path.Combine(Environment.CurrentDirectory, @"..\project.DAL\database");
		services.AddSingleton<IDbContextFactory<ProjectDbContext>>(provider => new DbContextSqLiteFactory(databaseFilePath));
		services.AddSingleton<IDbMigrator, SqliteDbMigrator>();

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
