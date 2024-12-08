using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Reflection;
using System.Text;
using ARM.Core.Identity.Providers;
using ARM.Core.Repositories;
using ARM.Core.Services.Security;
using ARM.Core.Services.Security.Impl;
using ARM.Core.Settings;
using ARM.Core.Validators.Entities;
using ARM.DAL.ApplicationContexts;
using ARM.DAL.Map;
using ARM.DAL.Repositories;
using ARM.WebApi.Extensions;
using ARM.WebApi.Identity.Providers;
using ARM.WebApi.Infrastructure.ModelBinders.Providers;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;

SetDefaultCulture();

var builder = WebApplication.CreateBuilder(args);

RegisterControllersWithServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.UseResponseCompression();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

WriteAppVersion();
RunMigrations();

app.Run();

void RegisterControllersWithServices()
{
    ConfigureLogger();

    builder.Services.AddResponseCompression(opt => opt.EnableForHttps = true);
    
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(x =>
    {
        x.SwaggerDoc("v1", new OpenApiInfo { Title = "AviagasResourceManagement API", Version = "v1" });
        x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description =
                "Get token from [POST] /login endpoint and paste it here with this template: Bearer {Your token}",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        x.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        });

    });
    
    builder.Services.AddAutoMapper(typeof(DatabaseAutoMapperProfile));

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
    builder.Services.AddTransient<IUserIdentityProvider, HttpUserIdentityProvider>();
    
    var configuration = builder.Configuration;
    var tokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JwtSettings:SigningKey"]!)),
        ValidAudience = configuration["JwtSettings:Audience"]!,
        ValidateAudience = true,
        ValidIssuer = configuration["JwtSettings:Issuer"]!,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
    builder.Services.AddSingleton(tokenValidationParameters);

    var jwtSettings = new JwtSettings()
    {
        SigningKey = configuration["JwtSettings:SigningKey"]!,
        AccessTokenLifetime = TimeSpan.Parse(configuration["JwtSettings:AccessTokenLifetime"]!),
        RefreshTokenMonthLifetime = int.Parse(configuration["JwtSettings:RefreshTokenMonthLifetime"]!),
        EncryptionKey = configuration["JwtSettings:EncryptionKey"]!,
        Audience = tokenValidationParameters.ValidAudience,
        Issuer = tokenValidationParameters.ValidIssuer,
        TokenValidationParameters = tokenValidationParameters
    };
    builder.Services.AddSingleton(jwtSettings);
    
    builder.Services.AddAuthorization();
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(jwt =>
    {
        jwt.SaveToken = true;
        jwt.TokenValidationParameters = tokenValidationParameters;
    });
    
    builder.Services.AddDbContext<AppDbContext>(opts =>
    {
        opts.UseNpgsql(configuration.GetConnectionString("Postgres"), options =>
        {
            options.SetPostgresVersion(new Version(17, 2, 0));
            options.MigrationsAssembly(typeof(AppDbContext).Assembly);
        });
        opts.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    });
    
    builder.Services.RegisterEntityRepositoriesFromAssembly();
    builder.Services.AddTransient<IAuthorizationRepository, EmployeesRepository>();
    builder.Services.AddTransient<IRefreshTokensRepository, RefreshTokensRepository>();
    builder.Services.AddTransient<IStatisticsRepository, StatisticsRepository>();
    
    builder.Services.AddTransient<IJwtService, JwtService>();

    builder.Services.AddValidatorsFromAssemblyContaining<EmployeeAccountValidator>();
    
    builder.Services.RegisterMediatR();
    
    builder.Services.AddTransient<IAuthorizationService, AuthorizationService>();
    
    builder.Services.AddControllers(opts => opts.ModelBinderProviders.Insert(0, new BaseListParamsModelBinderProvider()));
}

void SetDefaultCulture()
{
    var culture = new CultureInfo("ru-RU")
    {
        NumberFormat = { NumberDecimalSeparator = "." },
        DateTimeFormat = { ShortDatePattern = "dd.MM.yyyy", ShortTimePattern = "HH:mm:ss" }
    };

    CultureInfo.DefaultThreadCurrentCulture = culture;
    CultureInfo.DefaultThreadCurrentUICulture = culture;
}

void ConfigureLogger()
{
    builder.Logging.ClearProviders();
    
    LogManager.Setup().LoadConfigurationFromAppSettings();
    builder.Host.UseNLog();
}

void WriteAppVersion()
{
    logger.LogInformation($"Версия приложения: {Assembly.GetExecutingAssembly().GetName().Version}");
}

async void RunMigrations()
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var version = await dbContext.GetDatabaseVersion();
    logger.LogInformation($"Версия базы данных: {version}");
    
    logger.LogInformation("Запуск миграций...");
    //dbContext.Database.Migrate();
    logger.LogInformation("Миграции выполнены");
}