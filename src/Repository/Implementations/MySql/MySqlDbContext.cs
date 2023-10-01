using Microsoft.EntityFrameworkCore;
using Models.Entities.Timetables;
using Models.Entities.Timetables.Cells;
using Models.Entities.Users;
using MySqlConnector;

namespace Repository.Implementations.MySql;

#warning сделать доступ контекста потом internal
public class MySqlDbContext : DbContext
{
    public DbSet<Cabinet> Cabinets => Set<Cabinet>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<LessonTime> LessonTimes => Set<LessonTime>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<User> Users => Set<User>();

    private readonly MySqlConnection _mySqlConnection;

    public MySqlDbContext(MySqlConnection mySqlConnection)
    {
        _mySqlConnection = mySqlConnection;
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        try
        {
            var serverVer = ServerVersion.AutoDetect(_mySqlConnection);
            optionsBuilder.UseMySql(_mySqlConnection, serverVer);
        }
        catch (MySqlException ex)
        {
            throw new InvalidOperationException($"Не получилось открыть соединение с MySql.", ex);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("utf8mb4_unicode_ci").HasCharSet("utf8mb4");


        modelBuilder.Entity<Cabinet>(entity =>
        {
            entity.HasKey(c => c.CabinetPK).HasName("PRIMARY");
            entity.Property(c=>c.Address).HasMaxLength(255);
            entity.Property(c=>c.Number).HasMaxLength(255);
        });

        modelBuilder.Entity<LessonTime>(entity =>
        {
            entity.HasKey(lt => lt.LessonTimePK).HasName("PRIMARY");
            entity.Property(lt => lt.DayOfWeek).HasComment("Enum с днями недель.");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(s => s.SubjectPK).HasName("PRIMARY");
            entity.Property(s=>s.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(t => t.TeacherPK).HasName("PRIMARY");
            entity.Property(t => t.FirstName).HasMaxLength(255);
            entity.Property(t => t.MiddleName).HasMaxLength(255);
            entity.Property(t => t.Surname).HasMaxLength(255);
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(g => g.GroupPK).HasName("PRIMARY");
            entity.Property(g => g.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.UserPK).HasName("PRIMARY");
            entity.UseTphMappingStrategy();
            entity.Property<string>("Discriminator").HasMaxLength(30);
            entity.HasIndex(u => u.Email, "email-unique").IsUnique();
            entity.Property(u => u.Email).HasMaxLength(255);
        });

        modelBuilder.Entity<Administrator>(entity =>
        {
            entity.HasBaseType<User>();
        });
    }
}
