using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DAL.Factories
{
    public class DbContextSqLiteFactory : IDbContextFactory<ProjectDbContext>
    {
        private bool _seedTestingData;
        private readonly DbContextOptionsBuilder<ProjectDbContext> _contextOptionBuilder = new();

        public DbContextSqLiteFactory(string databaseName, bool seedTestingData = false)
        {
            _seedTestingData = seedTestingData;
            _contextOptionBuilder.UseSqlite($"Data Source={databaseName};Cache=Shared");
        }

        public ProjectDbContext CreateDbContext() => new(_contextOptionBuilder.Options, _seedTestingData);
    }
}
