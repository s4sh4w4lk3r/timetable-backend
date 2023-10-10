using Microsoft.EntityFrameworkCore;
using Models.Entities.Timetables;
using Models.Entities.Timetables.Cells;
using Models.Entities.Users;
using Throw;

namespace Repository.Implementations.MySql;

internal class SqlDbContext : DbContext
{
    public DbSet<Cabinet> Cabinets => Set<Cabinet>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<LessonTime> LessonTimes => Set<LessonTime>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Administrator> Administrators => Set<Administrator>();
    public DbSet<TimetableCell> TimetableCells => Set<TimetableCell>();
    public DbSet<Timetable> Timetables => Set<Timetable>();


    private readonly DbConfiguration _configuration;

    public SqlDbContext(DbConfiguration configuration)
    {
        configuration.ThrowIfNull();

        _configuration = configuration;
        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        try
        {
            switch (_configuration.DatabaseEngine)
            {
                case DatabaseEngine.MySql:
                    var serverVer = ServerVersion.AutoDetect(_configuration.ConnectionString);
                    optionsBuilder.UseMySql(_configuration.ConnectionString, serverVer);
                    break;

                case DatabaseEngine.PostgreSql:
                    optionsBuilder.UseNpgsql(_configuration.ConnectionString,
                    options => options.UseAdminDatabase("postgres"));
                    break;

                default:
                    throw new ArgumentException($"В контексте не реализована работа с типом БД {_configuration.DatabaseEngine}.");
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Не получилось открыть соединение с СУБД.", ex);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (string.IsNullOrWhiteSpace(_configuration.CharSet) is false)
        {
            modelBuilder.HasCharSet(_configuration.CharSet);
        }

        if (string.IsNullOrWhiteSpace(_configuration.DefaultSchema) is false)
        {
            modelBuilder.HasDefaultSchema(_configuration.DefaultSchema);
        }

        if (string.IsNullOrWhiteSpace(_configuration.Collation) is false)
        {
            modelBuilder.UseCollation(_configuration.Collation);
        }


        modelBuilder.Entity<Cabinet>(entity =>
        {
            entity.HasKey(c => c.CabinetId).HasName("CabinetPRIMARY");
            entity.Property(c => c.Address).HasMaxLength(255);
            entity.Property(c => c.Number).HasMaxLength(255);
        });

        modelBuilder.Entity<LessonTime>(entity =>
        {
            entity.HasKey(lt => lt.LessonTimeId).HasName("LessonTimePRIMARY");
            entity.Property(lt => lt.DayOfWeek).HasComment("Enum с днями недель.");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(s => s.SubjectId).HasName("SubjectPRIMARY");
            entity.Property(s => s.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(t => t.TeacherId).HasName("TeacherPRIMARY");
            entity.Property(t => t.FirstName).HasMaxLength(255);
            entity.Property(t => t.MiddleName).HasMaxLength(255);
            entity.Property(t => t.Surname).HasMaxLength(255);
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(g => g.GroupId).HasName("GroupPRIMARY");
            entity.Property(g => g.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.UserId).HasName("UserPRIMARY");
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

            entity.HasKey(t => t.TimeTableCellId).HasName("TimetableCellPRIMARY");

            entity.HasOne(e => e.Cabinet).WithMany(e => e.TimetableCells).IsRequired();
            entity.HasOne(e => e.Teacher).WithMany(e => e.TimetableCells).IsRequired();
            entity.HasOne(e => e.LessonTime).WithMany(e => e.TimetableCells).IsRequired();
            entity.HasOne(e => e.Subject).WithMany(e => e.TimetableCells).IsRequired();
        }
        );

        modelBuilder.Entity<Timetable>(entity =>
        {
            entity.HasKey(t => t.TimetableId).HasName("TimetablePRIMARY");
        }
        );
    }
}
