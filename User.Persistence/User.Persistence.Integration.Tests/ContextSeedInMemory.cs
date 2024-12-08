using User.Persistence.Infrastructure.RepositoryAccess;
using User.Persistence.Integration.Tests.Fakes.Entities;

namespace User.Persistence.Integration.Tests;
public class ContextSeedInMemory
{
    public static (Domain.Entities.User user, string password) Seed(TechChallengeContext context)
    {
        (var user, string password) = UserBuilder.Build();

        context.Users.Add(user);
        context.SaveChanges();

        return (user, password);
    }
}
