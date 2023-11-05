using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Entities.Timetables;
using Models.Entities.Timetables.Cells;
using Models.Entities.Timetables.Cells.CellMembers;
using Models.Entities.Users;
using Throw;
using static Repository.TimetableEntityBuilderMethods;
using static Repository.UserEntityBuilderMethods;

namespace Repository;

public class SqlDbContext : DbContext
{

    private readonly DbConfiguration _configuration;
    private readonly ILoggerFactory _loggerFactory;
    private static bool IsEnsureCreated = false;

    public SqlDbContext(IOptions<DbConfiguration> options, ILoggerFactory loggerFactory)
    {
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
    public DbSet<Timetable> Timetables => Set<Timetable>();
    public DbSet<ApprovalCode> ApprovalCodes => Set<ApprovalCode>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();
    public DbSet<EmailUpdateEntity> EmailUpdateEntities => Set<EmailUpdateEntity>();
    public DbSet<ActualTimetableCell> ActualTimetableCells => Set<ActualTimetableCell>();
    public DbSet<StableTimetableCell> StableTimetableCells => Set<StableTimetableCell>();
    public DbSet<StableTimetable> StableTimetables => Set<StableTimetable>();
    public DbSet<ActualTimetable> ActualTimetables => Set<ActualTimetable>();*/

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(_loggerFactory);

        try
        {
            optionsBuilder.UseNpgsql(_configuration.ConnectionString,
                    options => options.UseAdminDatabase(_configuration.PostgresAdminDbName));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Не получилось открыть соединение с СУБД.", ex);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (string.IsNullOrWhiteSpace(_configuration.DefaultSchema) is false)
        {
            modelBuilder.HasDefaultSchema(_configuration.DefaultSchema);
        }

        if (string.IsNullOrWhiteSpace(_configuration.Collation) is false)
        {
            modelBuilder.UseCollation(_configuration.Collation);
        }

        modelBuilder.Entity<User>(ConfigureUser);

        modelBuilder.Entity<ApprovalCode>(ConfigureApprovalCode);

        modelBuilder.Entity<UserSession>(ConfigureUserSession);

        modelBuilder.Entity<EmailUpdateEntity>(ConfigureEmailUpdateEntity);

        modelBuilder.Entity<Timetable>(ConfigureTimetable);

        modelBuilder.Entity<StableTimetable>(ConfigureStableTimetable);

        modelBuilder.Entity<ActualTimetable>(ConfigureActualTimetable);

        modelBuilder.Entity<TimetableCell>(ConfigureTimetableCell);

        modelBuilder.Entity<StableTimetableCell>(ConfigureStableTimetableCell);

        modelBuilder.Entity<ActualTimetableCell>(ConfigureActualTimetableCell);

        modelBuilder.Entity<LessonTime>(ConfigureLessonTime);

        modelBuilder.Entity<Cabinet>(ConfigureCabinet);

        modelBuilder.Entity<Subject>(ConfigureSubject);

        modelBuilder.Entity<Teacher>(ConfigureTeacher);

        modelBuilder.Entity<Group>(ConfigureGroup);
    }
}
