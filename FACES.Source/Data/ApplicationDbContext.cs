using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FACES.Models;


namespace FACES.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);  // to avoid an error with time type and postgresql
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<UserProject> UserProjects { get; set; }
    public DbSet<ProjectClient> ProjectClients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasMany(u => u.UserProjects)
            .WithOne(up => up.User)
            .HasForeignKey(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete

        modelBuilder.Entity<Project>()
            .HasMany(p => p.ProjectClients)
            .WithOne(pc => pc.Project)
            .HasForeignKey(pc => pc.ProjectId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete

        modelBuilder.Entity<Client>()
            .HasMany(c => c.ProjectClients)
            .WithOne(pc => pc.Client)
            .HasForeignKey(pc => pc.ClientId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete


        modelBuilder.Entity<UserProject>()
            .HasKey(up => new { up.UserId, up.ProjectId });  // Composite primary key

        modelBuilder.Entity<ProjectClient>()
            .HasKey(pc => new { pc.ProjectId, pc.ClientId });  // Composite primary key
    }
}
