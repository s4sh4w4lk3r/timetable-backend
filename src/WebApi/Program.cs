using FluentValidation;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;
using Models.Validation;
using Repository;
using Serilog;
using Throw;
using WebApi.Middlewares.Auth;
using WebApi.Services.Account.Implementations;
using WebApi.Services.Account.Interfaces;
using WebApi.Services.Timetables;
using WebApi.Types.Configuration;
using WebApi.Types.Validation;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        #region Настройка билдера.
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console()
        .WriteTo.File($"./logs/log.log", rollingInterval: RollingInterval.Day)
        .ReadFrom.Configuration(ctx.Configuration));

        builder.Services.AddControllers();

        builder.Services.AddAuthentication(AccessTokenAuthenticationOptions.DefaultScheme)
        .AddScheme<AccessTokenAuthenticationOptions, AccessTokenAuthenticationHandler>(AccessTokenAuthenticationOptions.DefaultScheme, options => { });

        #region Конфигурация
        builder.Services.Configure<DbConfiguration>(builder.Configuration.GetRequiredSection(nameof(DbConfiguration)));
        builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetRequiredSection(nameof(JwtConfiguration)));
        builder.Services.Configure<ApiSettings>(builder.Configuration.GetRequiredSection(nameof(ApiSettings)));
        builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetRequiredSection(nameof(EmailConfiguration)));
        #endregion

        #region Зависимости
        builder.Services.AddDbContext<SqlDbContext>();
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
        #endregion

        #region Валидаторы
        builder.Services.AddValidatorsFromAssemblyContaining<ApprovalCodeValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<CabinetValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<LessonTimeValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<TeacherValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<TimetableCellValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<TimetableValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<JwtConfigurationValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UserSessionValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<MailConfigurationValidator>();
        #endregion

        var app = builder.Build();
        #endregion

        #region Middlewares
        app.UseSerilogRequestLogging();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Use(CheckApiKey);

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.MapGet("/", () => "Hello World!");

        app.Run();
        #endregion
    }
    async static Task CheckApiKey(HttpContext context, Func<Task> next)
    {
        const string API_KEY = "Api-Key";

        string? apiKey = context.Request.Headers.Where(e => e.Key == API_KEY).Select(e => e.Value).FirstOrDefault();
        if (string.IsNullOrWhiteSpace(apiKey) is true)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync($"Для доступа к timetable-API требуется {API_KEY} в заголовках.");
            return;
        }

        string validApiKey = context.RequestServices.GetRequiredService<IOptions<ApiSettings>>().Value.ApiKey;
        validApiKey.ThrowIfNull().IfWhiteSpace();

        if (apiKey != validApiKey)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync($"Неверный {API_KEY}.");
            return;
        }

        await next.Invoke();
    }
}