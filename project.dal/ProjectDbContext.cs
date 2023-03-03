using Microsoft.EntityFrameworkCore;
using project.DAL.Entities;
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
        public DbSet<ActivityEntity> Activities => Set<ActivityEntity>();
        public DbSet<ActivityTagListEntity> ATLists => Set<ActivityTagListEntity>();
        public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();
        public DbSet<TagEntity> Tags => Set<TagEntity>();
        public DbSet<TodoEntity> Todos => Set<TodoEntity>();
        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<UserProjectListEntity> UPLists => Set<UserProjectListEntity>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=sqlitedb1; cache=shared");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
