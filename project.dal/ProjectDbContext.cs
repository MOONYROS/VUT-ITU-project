using Microsoft.EntityFrameworkCore;
using project.dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DAL
{
    public class ProjectDbContext : DbContext
    {
        private readonly bool _seedDemoData;

        public ProjectDbContext(DbContextOptions contextOptions, bool seedDemoData = false) 
            : base(contextOptions) => _seedDemoData = seedDemoData;
        public DbSet<Activity> Activities => Set<Activity>();
        public DbSet<ActivityTagList> ATLists => Set<ActivityTagList>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<Todo> Todos => Set<Todo>();
        public DbSet<User> Users => Set<User>();
        public DbSet<UserProjectList> UPLists => Set<UserProjectList>();
    }
}
