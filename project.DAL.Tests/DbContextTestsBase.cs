using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace project.DAL.Tests
{
    public class DbContextTestsBase : IAsyncLifetime
    {
        protected DbContextTestsBase()
        {
            DbContextFactory = new DbContextSqLiteTestingFactory(GetType().FullName!);

            ProjectDbContextSUT = DbContextFactory.CreateDbContext();
        }

        protected IDbContextFactory<ProjectDbContext> DbContextFactory { get; }
        protected ProjectDbContext ProjectDbContextSUT { get; }

        public async Task InitializeAsync()
        {
            await ProjectDbContextSUT.Database.EnsureDeletedAsync();
            await ProjectDbContextSUT.Database.EnsureCreatedAsync();
        }

        public async Task DisposeAsync()
        {
            await ProjectDbContextSUT.Database.EnsureDeletedAsync();
            await ProjectDbContextSUT.DisposeAsync();
        }
    }
}
