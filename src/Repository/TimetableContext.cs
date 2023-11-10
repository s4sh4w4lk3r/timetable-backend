using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Entities.Timetables;
using Models.Entities.Timetables.Cells;
using Models.Entities.Timetables.Cells.CellMembers;
using Throw;
using static Repository.TimetableSchemaMethods;
using static Repository.AccountingSchemaMethods;

namespace Repository;

public class TimetableContext : DbContext
{
    private readonly DbConfiguration _configuration;
    private readonly ILoggerFactory _loggerFactory;
    private static bool IsEnsureCreated = false;

    public TimetableContext(IOptions<DbConfiguration> options, ILoggerFactory loggerFactory)
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(_loggerFactory);
        optionsBuilder.UseNpgsql(_configuration.ConnectionString,
                   options => options.UseAdminDatabase(_configuration.PostgresAdminDbName));
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
        #region Настройка схема accounting

        /*        modelBuilder.Entity<User>(ConfigureUser);

                modelBuilder.Entity<ApprovalCode>(ConfigureApprovalCode);

                modelBuilder.Entity<UserSession>(ConfigureUserSession);

                modelBuilder.Entity<EmailUpdateEntity>(ConfigureEmailUpdateEntity);*/
        #endregion


        #region Настройка схемы timetable
        modelBuilder.Entity<StableTimetable>(ConfigureStableTimetable);

        modelBuilder.Entity<ActualTimetable>(ConfigureActualTimetable);

        modelBuilder.Entity<StableTimetableCell>(ConfigureStableTimetableCell);

        modelBuilder.Entity<ActualTimetableCell>(ConfigureActualTimetableCell);

        modelBuilder.Entity<LessonTime>(ConfigureLessonTime);

        modelBuilder.Entity<Cabinet>(ConfigureCabinet);

        modelBuilder.Entity<Subject>(ConfigureSubject);

        modelBuilder.Entity<Teacher>(ConfigureTeacher);

        modelBuilder.Entity<Group>(ConfigureGroup);
        #endregion
    }
}
