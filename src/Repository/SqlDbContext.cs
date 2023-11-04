using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Entities.Timetables;
using Models.Entities.Timetables.Cells;
using Models.Entities.Timetables.Cells.CellMembers;
using Models.Entities.Users;
using Throw;

namespace Repository;

public class SqlDbContext : DbContext
{

    private readonly DbConfiguration _configuration;
    private readonly ILoggerFactory _loggerFactory;
    private static bool IsEnsureCreated = false;

    public SqlDbContext(IOptions<DbConfiguration> options, ILoggerFactory loggerFactory)
    {
        new DbConfigurationValidator().ValidateAndThrow(options.Value);
        loggerFactory.ThrowIfNull();

        _loggerFactory = loggerFactory;
        _configuration = options.Value;

        if (IsEnsureCreated is false)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
            IsEnsureCreated = true;
        }
    }

    /*public DbSet<Cabinet> Cabinets => Set<Cabinet>();
    public DbSet<LessonTime> LessonTimes => Set<LessonTime>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<User> Users => Set<User>();
    public DbSet<TimetableCell> TimetableCells => Set<TimetableCell>();
    public DbSet<Timetable> Timetables => Set<Timetable>();
    public DbSet<ApprovalCode> ApprovalCodes => Set<ApprovalCode>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();
    public DbSet<EmailUpdateEntity> EmailUpdateEntities => Set<EmailUpdateEntity>();*/

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(_loggerFactory);

        try
        {
            switch (_configuration.DatabaseEngine)
            {
                case DatabaseEngine.MySql:
                    var serverVer = ServerVersion.AutoDetect(_configuration.ConnectionString);
                    optionsBuilder.UseMySql(_configuration.ConnectionString, serverVer);
                    break;

                case DatabaseEngine.PostgreSql:
                    _configuration.PostgresAdminDbName.ThrowIfNull().IfWhiteSpace();

                    optionsBuilder.UseNpgsql(_configuration.ConnectionString,
                    options => options.UseAdminDatabase(_configuration.PostgresAdminDbName));
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
            entity.HasIndex(u => u.Email, "email-unique").IsUnique();
            entity.Property(u => u.Email).HasMaxLength(255).IsRequired();
            entity.Property(u => u.Password).HasMaxLength(72).IsRequired();
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

        modelBuilder.Entity<ApprovalCode>(entity =>
        {
            entity.HasKey(e => e.AprrovalCodeId).HasName("ApprovalCodePRIMARY");
            entity.HasOne(e => e.User).WithMany(e => e.ApprovalCodes).HasForeignKey(e=>e.UserId).IsRequired();
        });

        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.HasKey(e => e.UserSessionId).HasName("UserSessionPRIMARY");
            entity.HasOne(e => e.User).WithMany(e => e.UserSessions).HasForeignKey(e=>e.UserId).IsRequired();
        });

        modelBuilder.Entity<EmailUpdateEntity>(entity =>
        {
            entity.HasKey(e => e.EmailUpdateEntityId).HasName("EmailUpdateEntityPRIMARY");
            entity.HasOne(e=>e.User).WithMany(e=>e.EmailUpdateEntities).HasForeignKey(e=>e.UserId).IsRequired();
            entity.HasOne(e => e.Approval).WithOne(e => e.EmailUpdateEntity).IsRequired();
            entity.Property(e => e.OldEmail).IsRequired().HasMaxLength(255);
            entity.Property(e => e.NewEmail).IsRequired().HasMaxLength(255);
        });
    }
}
