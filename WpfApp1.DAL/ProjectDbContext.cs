using Microsoft.EntityFrameworkCore;
using WpfApp1.DAL.Entities;

namespace WpfApp1.DAL;

public class ProjectDbContext : DbContext
{
    private readonly bool _seedDemoData;

    public ProjectDbContext(DbContextOptions contextOptions, bool seedDemoData = false)
        : base(contextOptions) => _seedDemoData = seedDemoData;
    public DbSet<ActivityEntity> Activities => Set<ActivityEntity>();
    public DbSet<ActivityTagListEntity> ATLists => Set<ActivityTagListEntity>();
    public DbSet<UserActivityListEntity> UALists => Set<UserActivityListEntity>();
    public DbSet<TagEntity> Tags => Set<TagEntity>();
    public DbSet<TodoEntity> Todos => Set<TodoEntity>();
    public DbSet<UserEntity> Users => Set<UserEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
            
        // modelBuilder.Entity<UserEntity>()
        //     .HasMany(i => i.Todos)
        //     .WithOne(i => i.User)
        //     .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserEntity>()
            .HasMany(i => i.Activities)
            .WithOne(i => i.User)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<UserEntity>()
            .HasMany(i => i.Tags)
            .WithOne(i => i.User)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TagEntity>()
            .HasMany(i=>i.Activities)
            .WithOne(i => i.Tag)
            .OnDelete(DeleteBehavior.Cascade); 
        
        modelBuilder.Entity<TagEntity>()
            .HasOne(i => i.User)
            .WithMany(i => i.Tags)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<ActivityEntity>()
            .HasMany(i=>i.Tags)
            .WithOne(i => i.Activity)
            .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<ActivityEntity>()
            .HasMany(i => i.Users)
            .WithOne(i => i.Activity)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TodoEntity>()
            .HasOne(i=>i.User)
            .WithMany(i => i.Todos)
            .OnDelete(DeleteBehavior.Cascade);
    }
}