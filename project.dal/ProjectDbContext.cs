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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>()
                .HasMany(i => i.Projects)
                .WithOne(i => i.User)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<UserEntity>()
                .HasMany(i=>i.Todos)
                .WithOne(i => i.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectEntity>()
                .HasMany<UserProjectListEntity>()
                .WithOne(i => i.Project)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TagEntity>()
                .HasMany<ActivityTagListEntity>()
                .WithOne(i => i.Tag)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ActivityEntity>()
                .HasMany<ActivityTagListEntity>()
                .WithOne(i => i.Activity)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TodoEntity>()
                .HasOne(i=>i.User)
                .WithMany(i => i.Todos)
                .OnDelete(DeleteBehavior.Cascade);
        }
        
        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=sqlitedb1; cache=shared");
            base.OnConfiguring(optionsBuilder);
        }
        */
    }
}
