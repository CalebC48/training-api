using Microsoft.EntityFrameworkCore;

namespace CAP.API.Models;

public class TrainingContext: DbContext
{
    /// <summary>
    /// Creates a table named Users
    /// </summary>
    public DbSet<User> Users {get; set;}
    /// <summary>
    /// Creates a table named Roles
    /// </summary>
    public DbSet<Role> Roles {get; set;}

    /// <summary>
    /// Constructor for the TrainingContext
    /// </summary>
    public TrainingContext() {}
    /// <summary>
    /// Alternate Constructor
    /// </summary>
    /// <param name ="options"></param>
    public TrainingContext(DbContextOptions<TrainingContext> options) : base(options) {}

    /// <summary>
    /// Creates the database tables with the Timestamped property
    /// </summary>
    /// <param name ="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().UseTimestampedProperty();
        modelBuilder.Entity<Role>().UseTimestampedProperty();
    }
}