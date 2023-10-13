using FluentValidation;
using Microsoft.AspNetCore.HttpOverrides;
using Models.Validation;
using Repository;
using Repository.Implementations.EFCore;
using Repository.Interfaces;
using Serilog;
using WebApi.ConfigurationTypes;
using WebApi.ConfigurationTypes.Validation;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console()
            .WriteTo.File($"./logs/log.log", rollingInterval: RollingInterval.Day)
            .ReadFrom.Configuration(ctx.Configuration));

            builder.Services.Configure<DbConfiguration>(builder.Configuration.GetRequiredSection(nameof(DbConfiguration)));
            builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetRequiredSection(nameof(JwtConfiguration)));

            builder.Services.AddScoped(typeof(IRepository<>), typeof(EFCoreRepository<>));

            builder.Services.AddValidatorsFromAssemblyContaining<ApprovalCodeValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<CabinetValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<GroupValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<LessonTimeValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<SubjectValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<TeacherValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<TimetableCellValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<TimetableValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<JwtConfigurationValidator>();

            var app = builder.Build();

            app.UseSerilogRequestLogging();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}