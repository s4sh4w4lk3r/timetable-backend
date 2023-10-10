using FluentValidation;

namespace Repository;

public class DbConfigurationValidator : AbstractValidator<DbConfiguration>
{
    public DbConfigurationValidator()
    {
        RuleFor(e=>e.ConnectionString).NotEmpty();
        RuleFor(e=>e.DatabaseEngineName).NotEmpty();
        RuleFor(e => e.DatabaseEngine).IsInEnum();
        RuleFor(e=>e.PostgresAdminDbName).NotEmpty();
    }
}
