using FluentValidation;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Models.Validation;
using Repository;
using Serilog;
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
        #region ��������� �������.
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console()
        .WriteTo.File($"./logs/log.log", rollingInterval: RollingInterval.Day)
        .ReadFrom.Configuration(ctx.Configuration));

        builder.Services.AddControllers();

        builder.Services.AddAuthentication(AccessTokenAuthenticationOptions.DefaultScheme)
        .AddScheme<AccessTokenAuthenticationOptions, AccessTokenAuthenticationHandler>(AccessTokenAuthenticationOptions.DefaultScheme, options => { });

        builder.Services.Configure<DbConfiguration>(builder.Configuration.GetRequiredSection(nameof(DbConfiguration)));
        builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetRequiredSection(nameof(JwtConfiguration)));

        builder.Services.AddScoped<DbContext, SqlDbContext>();
        builder.Services.AddScoped<CabinetService>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<ApprovalService>();
        builder.Services.AddTransient<IEmailClient, EmailSimulator>();
        builder.Services.AddScoped<UserSessionService>();

        builder.Services.AddValidatorsFromAssemblyContaining<ApprovalCodeValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<CabinetValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<LessonTimeValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<TeacherValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<TimetableCellValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<TimetableValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<JwtConfigurationValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UserSessionValidator>();

        var app = builder.Build();
        #endregion

        app.UseSerilogRequestLogging();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.MapGet("/", () => "Hello World!");

        app.Run();
    }
}