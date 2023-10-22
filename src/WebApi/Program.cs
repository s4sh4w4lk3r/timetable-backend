﻿using FluentValidation;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
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

        builder.Services.Configure<DbConfiguration>(builder.Configuration.GetRequiredSection(nameof(DbConfiguration)));
        builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetRequiredSection(nameof(JwtConfiguration)));
        builder.Services.Configure<ApiSettings>(builder.Configuration.GetRequiredSection(nameof(ApiSettings)));

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
        app.Use(CheckApiKey);

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.MapGet("/", () => "Hello World!");

        app.Run();
    }
    async static Task CheckApiKey(HttpContext context, Func<Task> next)
    {
        const string API_KEY = "Api-Key";

        string? apiKey = context.Request.Headers.Where(e => e.Key == API_KEY).Select(e => e.Value).FirstOrDefault();
        if (string.IsNullOrWhiteSpace(apiKey) is true)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync($"Для доступа к timetable-API требуется {API_KEY} в заголовках.");
            return;
        }

        string validApiKey = context.RequestServices.GetRequiredService<IOptions<ApiSettings>>().Value.ApiKey;
        validApiKey.ThrowIfNull().IfWhiteSpace();

        if (apiKey != validApiKey)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync($"Неверный {API_KEY}.");
            return;
        }
        

        await next.Invoke();
    }
}
