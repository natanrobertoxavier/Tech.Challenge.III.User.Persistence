using User.Persistence.Domain.Repositories.User;

namespace User.Persistence.Infrastructure.RepositoryAccess.User;
public class UserRepository(TechChallengeContext context) : IUserUpdateOnlyRepository
{
    private readonly TechChallengeContext _context = context;

    public void Update(Domain.Entities.User user) =>
        _context.Users.Update(user);
}
