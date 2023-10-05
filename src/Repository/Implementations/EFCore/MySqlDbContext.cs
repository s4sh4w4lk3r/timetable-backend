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
    public DbSet<Administrator> Administrators => Set<Administrator>();
    public DbSet<TimetableCell> TimetableCells => Set<TimetableCell>();
    public DbSet<Timetable> Timetables => Set<Timetable>();


    private readonly MySqlConnection _mySqlConnection;

    public MySqlDbContext(MySqlConnection mySqlConnection)
    {
        _mySqlConnection = mySqlConnection;
        //Database.EnsureDeleted();
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
            entity.HasKey(c => c.CabinetId).HasName("PRIMARY");
            entity.Property(c=>c.Address).HasMaxLength(255);
            entity.Property(c=>c.Number).HasMaxLength(255);
        });

        modelBuilder.Entity<LessonTime>(entity =>
        {
            entity.HasKey(lt => lt.LessonTimeId).HasName("PRIMARY");
            entity.Property(lt => lt.DayOfWeek).HasComment("Enum с днями недель.");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(s => s.SubjectId).HasName("PRIMARY");
            entity.Property(s=>s.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(t => t.TeacherId).HasName("PRIMARY");
            entity.Property(t => t.FirstName).HasMaxLength(255);
            entity.Property(t => t.MiddleName).HasMaxLength(255);
            entity.Property(t => t.Surname).HasMaxLength(255);
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(g => g.GroupId).HasName("PRIMARY");
            entity.Property(g => g.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.UserId).HasName("PRIMARY");
            entity.UseTphMappingStrategy();
            entity.Property<string>("Discriminator").HasMaxLength(30);
            entity.HasIndex(u => u.Email, "email-unique").IsUnique();
            entity.Property(u => u.Email).HasMaxLength(255);
        });

        modelBuilder.Entity<Administrator>(entity =>
        {
            entity.HasBaseType<User>();
        });

        modelBuilder.Entity<TimetableCell>(entity =>
        {

            entity.HasKey(t => t.TimeTableCellId).HasName("PRIMARY");

            entity.HasOne(e => e.Cabinet).WithMany(e => e.TimetableCells).IsRequired();
            entity.HasOne(e => e.Teacher).WithMany(e => e.TimetableCells).IsRequired();
            entity.HasOne(e => e.LessonTime).WithMany(e => e.TimetableCells).IsRequired();
            entity.HasOne(e => e.Subject).WithMany(e => e.TimetableCells).IsRequired();
        }
        );

        modelBuilder.Entity<Timetable>(entity =>
        {
            entity.HasKey(t => t.TimetableId).HasName("PRIMARY");
        }
        );
    }
}
