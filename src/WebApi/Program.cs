using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models.Validation;
using Repository;
using Serilog;
using Services.Implementations;
using Services.Interfaces;
using WebApi.Services.Implementations;
using WebApi.Services.Implementations.Timetables;
using WebApi.Services.Interfaces;
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

        var jwtConfigurationSection = builder.Configuration.GetRequiredSection(nameof(JwtConfiguration));
        var jwtConfigurationValue = jwtConfigurationSection
            #error опять не вытаскивается конфигурация
#error также надо пересоздать бд.
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtConfigurationValue.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtConfigurationValue.Audience,
                ValidateLifetime = true,
                IssuerSigningKey = jwtConfigurationValue.GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = true
            };
        });

            builder.Services.Configure<DbConfiguration>(builder.Configuration.GetRequiredSection(nameof(DbConfiguration)));
        builder.Services.Configure<JwtConfiguration>(jwtConfigurationSection);

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
}