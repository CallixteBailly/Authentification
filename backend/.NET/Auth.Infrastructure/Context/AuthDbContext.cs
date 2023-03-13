
using Auth.Domain.Entities;
using Auth.Infrastructure.Persistance.Interceptros;
using Auth.Application.Interface;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Context;
public class AuthDbContext : DbContext, IAuthDbContext
{
    private readonly AuditableEntitySaveChangesInterceptor? _auditableEntitySaveChangesInterceptor;
    public AuthDbContext() : base()
    {
    }
    public AuthDbContext(
        DbContextOptions<AuthDbContext> options,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
        : base(options)
    {
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }
    public DbSet<User>? Users => Set<User>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_auditableEntitySaveChangesInterceptor != null)
            optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }
}