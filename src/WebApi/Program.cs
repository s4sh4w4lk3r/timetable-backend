using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;
using Repository;
using Serilog;
using System.Reflection;
using WebApi.GraphQL.EnumTypes;
using WebApi.GraphQL.ObjectTypes;
using WebApi.GraphQL.OperationTypes;
using WebApi.Middlewares.Authentication;
using WebApi.Services.Identity.Implementations;
using WebApi.Services.Identity.Interfaces;
using WebApi.Services.Timetables.Implementations;
using WebApi.Services.Timetables.Interfaces;
using WebApi.Types.Configuration;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console()
        .WriteTo.File($"./logs/log.log", rollingInterval: RollingInterval.Day)
        .ReadFrom.Configuration(ctx.Configuration));

        ConfigureServices(builder);
        ConfigureGraphQL(builder);
        ConfigureDependencies(builder);
        ConfigureIOptions(builder);

        var app = builder.Build();
        ConfigureMiddlewares(app);

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<TimetableContext>();
            //db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Dispose();
        }
        app.Run();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Timetable WebApi",
                Description = "Timetable WebApi"
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        builder.Services.AddAuthentication(AccessTokenAuthenticationOptions.DefaultScheme)
         .AddScheme<AccessTokenAuthenticationOptions, AccessTokenAuthenticationHandler>(AccessTokenAuthenticationOptions.DefaultScheme, _ => { });
        builder.Services.AddAuthorization();

        
    }
    private static void ConfigureDependencies(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<TimetableContext>(contextLifetime: ServiceLifetime.Scoped, optionsLifetime: ServiceLifetime.Scoped);
        builder.Services.AddScoped<IEmailUpdater, EmailUpdater>();
        builder.Services.AddScoped<IPasswordService, PasswordService>();
        builder.Services.AddScoped<IRegistrationService, RegistrationService>();
        builder.Services.AddScoped<IUnregistrationService, UnregistrationService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IUserSessionService, UserSessionService>();
        builder.Services.AddScoped<IApprovalService, ApprovalService>();
        builder.Services.AddScoped<IApprovalSender, ApprovalSender>();
        builder.Services.AddScoped<IRegistrationEntityService, RegistrationEntityService>();
        builder.Services.AddScoped<IStableTimetableService, StableTimetableService>();
        builder.Services.AddScoped<IActualTimetableService, ActualTimetableService>();
        builder.Services.AddScoped<IActualCellEditor, ActualCellEditor>();

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddTransient<IEmailClient, EmailSimulator>();
        }
        else
        {
            builder.Services.AddTransient<IEmailClient, MailKitClient>();
        }
    }
    private static void ConfigureIOptions(WebApplicationBuilder builder)
    {
        builder.Services.Configure<DbConfiguration>(builder.Configuration.GetRequiredSection(nameof(DbConfiguration)));
        builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetRequiredSection(nameof(JwtConfiguration)));
        builder.Services.Configure<ApiSettings>(builder.Configuration.GetRequiredSection(nameof(ApiSettings)));
        builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetRequiredSection(nameof(EmailConfiguration)));
        builder.Services.Configure<KestrelServerOptions>(options =>
        {
            options.AllowSynchronousIO = true;
        });
    }
    private static void ConfigureMiddlewares(WebApplication app)
    {

        app.UseSerilogRequestLogging();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapGraphQL();
        app.MapControllers();

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
    private static void ConfigureGraphQL(WebApplicationBuilder builder)
    {
#warning ограничить браузинг схемы.
        builder.Services.AddGraphQLServer()
            .ModifyOptions(options => { options.DefaultBindingBehavior = BindingBehavior.Explicit; })
            .AddQueryType<QueryType>()
           //.AddMutationType<Mutations>()
           .AddType<ActualTimetableType>()
           .AddType<SubGroupType>()
           .AddType<GroupType>()
           .AddType<ActualTimetableCellType>()
           .AddType<TeacherType>()
           .AddType<SubjectType>()
           .AddType<CabinetType>()
           .AddType<LessonTimeType>()
           .AddType<StableTimetableType>()
           .AddType<StableTimetableCellType>()
           .AddType<DayOfWeekType>()
           .AddProjections()
           .AddFiltering()
           .AddSorting()
           .AddAuthorization();
#warning изучить даталоудеры или еще как-то решить проблему с query, так как два query запроса вызовут исключение
    }
}
#warning почему-то при транкейте таблиц каскадно в схеме таймтейблов, удаляются что-то из схемы идентити и юзеры не могут войти.