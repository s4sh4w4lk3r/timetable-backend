using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Core.Entities.Timetables;
using Core.Entities.Timetables.Cells;
using Core.Entities.Timetables.Cells.CellMembers;
using Throw;
using static Repository.TimetableSchemaMethods;
using static Repository.IdentitySchemaMethods;
using Core.Entities.Identity.Users;
using Core.Entities.Identity;

namespace Repository;

public class TimetableContext : DbContext
{
    private readonly DbConfiguration _configuration;
    private readonly ILoggerFactory _loggerFactory;

    public TimetableContext(IOptions<DbConfiguration> options, ILoggerFactory loggerFactory)
    {
        loggerFactory.ThrowIfNull();

        _loggerFactory = loggerFactory;
        _configuration = options.Value;
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
        #region Настройка схемы identity

        modelBuilder.Entity<User>(ConfigureUser);
        modelBuilder.Entity<Teacher>(ConfigureTeacher);
        modelBuilder.Entity<Admin>(ConfigureAdmin);
        modelBuilder.Entity<Student>(ConfigureStudent);
        modelBuilder.Entity<UserSession>(ConfigureUserSession);
        modelBuilder.Entity<Approval>(ConfigureApproval);
        modelBuilder.Entity<EmailUpdateEntity>(ConfigureEmailUpdateEntity);
        modelBuilder.Entity<RegistrationEntity>(ConfigureRegistrationEntity);
        #endregion


        #region Настройка схемы timetable
        modelBuilder.Entity<StableTimetable>(ConfigureStableTimetable);

        modelBuilder.Entity<ActualTimetable>(ConfigureActualTimetable);

        modelBuilder.Entity<StableTimetableCell>(ConfigureStableTimetableCell);

        modelBuilder.Entity<ActualTimetableCell>(ConfigureActualTimetableCell);

        modelBuilder.Entity<LessonTime>(ConfigureLessonTime);

        modelBuilder.Entity<Cabinet>(ConfigureCabinet);

        modelBuilder.Entity<Subject>(ConfigureSubject);

        modelBuilder.Entity<TeacherCM>(ConfigureTeacher);

        modelBuilder.Entity<Group>(ConfigureGroup);
        #endregion
    }
}
