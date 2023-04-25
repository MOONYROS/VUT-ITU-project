using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DAL.Factories
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ProjectDbContext>
    {

        private const string connectionString = $"DataSource=Project;Cache=Shared";
        private readonly DbContextSqLiteFactory _dbContextSqLiteFactory;

        public DesignTimeDbContextFactory() 
        {
            _dbContextSqLiteFactory = new DbContextSqLiteFactory(connectionString);
        }
        public ProjectDbContext CreateDbContext(string[] args) => _dbContextSqLiteFactory.CreateDbContext();
    }
}
