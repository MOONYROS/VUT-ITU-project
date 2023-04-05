using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using project.DAL;

namespace project.DAL.Tests
{
    public class DbContextSqLiteTestingFactory : IDbContextFactory<ProjectDbContext>
    {
        private readonly string _databaseName;

        public DbContextSqLiteTestingFactory(string databaseName)
        {
            _databaseName = databaseName;
        }
        
        public ProjectDbContext CreateDbContext() 
        {
            DbContextOptionsBuilder<ProjectDbContext> builder = new();
            builder.UseSqlite($"Data Source={_databaseName};Cache=Shared");

            return new ProjectDbContext(builder.Options);
        }
    }
}
