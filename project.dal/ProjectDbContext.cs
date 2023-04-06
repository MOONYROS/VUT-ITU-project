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
        public DbSet<ActivityTagListEntity> AtLists => Set<ActivityTagListEntity>();
        public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();
        public DbSet<TagEntity> Tags => Set<TagEntity>();
        public DbSet<TodoEntity> Todos => Set<TodoEntity>();
        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<UserProjectListEntity> UpLists => Set<UserProjectListEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TODO: DeleteBehavior should be restrict or cascade
            modelBuilder.Entity<UserEntity>()
                .HasMany(i => i.Projects)
                .WithOne(i => i.User)
                .OnDelete(DeleteBehavior.Restrict); // Deleting user shouldn't delete project
            
            modelBuilder.Entity<UserEntity>()
                .HasMany(i=>i.Todos)
                .WithOne(i => i.User)
                .OnDelete(DeleteBehavior.Cascade); // Deleting user will delete all his Todos

            modelBuilder.Entity<UserEntity>()
                .HasMany(i => i.Activities)
                .WithOne(i => i.User)
                // TODO: Should deleting user delete activities that are part of projects?
                // Possible solution might be ProjectActivityEntity and UserActivityEntity separately
                .OnDelete(DeleteBehavior.Cascade); // Cascade is fine in the meanwhile

            modelBuilder.Entity<ProjectEntity>()
                .HasMany(i=>i.Users)
                .WithOne(i => i.Project)
                .OnDelete(DeleteBehavior.Restrict); // Deleting project shouldnt delete users

            modelBuilder.Entity<ProjectEntity>()
                .HasMany(i => i.Activities)
                .WithOne(i => i.Project)
                .OnDelete(DeleteBehavior.Cascade); // Deleting project should delete its activities

            modelBuilder.Entity<TagEntity>()
                .HasMany(i=>i.Activities)
                .WithOne(i => i.Tag)
                .OnDelete(DeleteBehavior.Restrict); // Deleting tag shouldnt delete activities
            
            modelBuilder.Entity<ActivityEntity>()
                .HasMany(i=>i.Tags)
                .WithOne(i => i.Activity)
                .OnDelete(DeleteBehavior.Restrict); // Deleting activity shouldnt delete tags

            modelBuilder.Entity<ActivityEntity>()
                .HasOne(i => i.User)
                .WithMany(i => i.Activities)
                .OnDelete(DeleteBehavior.Restrict); // Deleting activity shouldnt delete user

            modelBuilder.Entity<ActivityEntity>()
                .HasOne(i => i.Project)
                .WithMany(i => i.Activities)
                .OnDelete(DeleteBehavior.Restrict); // Deleting activity shouldnt delete project

            modelBuilder.Entity<TodoEntity>()
                .HasOne(i=>i.User)
                .WithMany(i => i.Todos)
                .OnDelete(DeleteBehavior.Restrict); // Deleting to do should delete user
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
