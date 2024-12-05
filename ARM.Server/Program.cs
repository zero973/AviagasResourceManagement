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
app.UseAuthorization();
app.MapControllers();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

WriteAppVersion();
RunMigrations();

app.Run();

void RegisterControllersWithServices()
{
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
    
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
        ValidAudience = "ARM",
        ValidateAudience = true,
        ValidIssuer = "ARM.Security.Service",
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
        Audience = "ARM",
        Issuer = "ARM.Security.Service",
        TokenValidationParameters = tokenValidationParameters
    };
    builder.Services.AddSingleton(jwtSettings);
    
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
    builder.Services.AddAuthorization();
    
    builder.Services.AddDbContext<AppDbContext>(opts =>
    {
        opts.UseNpgsql(configuration.GetConnectionString("Postgres"), options =>
        {
            options.SetPostgresVersion(new Version(17, 1, 0));
            options.MigrationsAssembly(typeof(AppDbContext).Assembly);
        });
        opts.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    });
    
    builder.Services.RegisterEntityRepositoriesFromAssembly();
    builder.Services.AddTransient<IRefreshTokensRepository, RefreshTokensRepository>();
    
    builder.Services.AddTransient<IJwtService, JwtService>();

    builder.Services.AddValidatorsFromAssemblyContaining<AppUserValidator>();
    
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

void WriteAppVersion()
{
    logger.LogInformation($"Версия приложения: {Assembly.GetExecutingAssembly().GetName().Version}");
}

void RunMigrations()
{
    logger.LogInformation("Запуск миграций...");

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
    
    logger.LogInformation("Миграции выполнены");
}