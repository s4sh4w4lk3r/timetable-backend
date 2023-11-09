using FluentValidation;
using Microsoft.AspNetCore.HttpOverrides;
using Repository;
using Serilog;
using Validation;
using WebApi.GraphQL;
using WebApi.Middlewares.Auth;
using WebApi.Services.Account.Implementations;
using WebApi.Services.Account.Interfaces;
using WebApi.Services.Timetables;
using WebApi.Services.Timetables.CellMembers;
using WebApi.Types.Configuration;
using WebApi.Types.Validation;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console()
        .WriteTo.File($"./logs/log.log", rollingInterval: RollingInterval.Day)
        .ReadFrom.Configuration(ctx.Configuration));

        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAuthentication(AccessTokenAuthenticationOptions.DefaultScheme)
        .AddScheme<AccessTokenAuthenticationOptions, AccessTokenAuthenticationHandler>(AccessTokenAuthenticationOptions.DefaultScheme, options => { });
        builder.Services.AddGraphQLServer()
            .AddQueryType<Queries>()
            .AddProjections()
            .AddFiltering()
            .AddSorting();

        #region Конфигурация
        builder.Services.Configure<DbConfiguration>(builder.Configuration.GetRequiredSection(nameof(DbConfiguration)));
        builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetRequiredSection(nameof(JwtConfiguration)));
        builder.Services.Configure<ApiSettings>(builder.Configuration.GetRequiredSection(nameof(ApiSettings)));
        builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetRequiredSection(nameof(EmailConfiguration)));
        #endregion

        #region Зависимости
        builder.Services.AddDbContext<TimetableContext>();
        builder.Services.AddScoped<CabinetService>();
        builder.Services.AddScoped<EmailUpdater>();
        builder.Services.AddScoped<PasswordService>();
        builder.Services.AddScoped<IRegistrationService, RegistrationService>();
        builder.Services.AddScoped<IUnregistrationService, UnregistrationService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        //builder.Services.AddTransient<IEmailClient, MailKitClient>();
        builder.Services.AddTransient<IEmailClient, EmailSimulator>();
        builder.Services.AddScoped<IUserSessionService, UserSessionService>();
        builder.Services.AddScoped<IApprovalService, ApprovalService>();
        builder.Services.AddScoped<IApprovalSender, ApprovalSender>();
        builder.Services.AddScoped<ActualTimetableService>();
        #endregion

        builder.Services.AddValidatorsFromAssemblyContaining<CabinetValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<TeacherValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<JwtConfigurationValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UserSessionValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<MailConfigurationValidator>();

        var app = builder.Build();

        #region Middlewares
        app.UseSerilogRequestLogging();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapGraphQL();
        app.MapControllers();

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.MapGet("/", () => "Hello World!");

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
#warning нормально описать сваггер.
        }

        app.Run();
        #endregion
    }
}