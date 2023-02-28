
namespace Auth.Application.Interface;
public interface IAuthDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

