using System.Text;
using Auth.Application.Interface;
using Auth.Infrastructure.Authentication;
using Auth.Infrastructure.Context;
using Auth.Infrastructure.Persistance;
using Auth.Infrastructure.Persistance.Interceptros;
using Auth.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Auth.Infrastructure;

public static class DependencyInjeciton
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        services.AddUserConnetion(configuration);
        services.AddAuthentitcation(configuration);
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthDbContext>(provider => provider.GetRequiredService<AuthDbContext>());
        services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }
    private static IServiceCollection AddAuthentitcation(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var JwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, JwtSettings);

        services.AddSingleton(Options.Create(JwtSettings));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = JwtSettings.Issuer,
                ValidAudience = JwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(JwtSettings.Secret))
            });
        return services;
    }
    private static IServiceCollection AddUserConnetion(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        services.AddDbContext<AuthDbContext>(options =>
            options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Auth.API")));
        return services;
    }
}
