namespace User.Persistence.Domain.Repositories.User;
public interface IUserUpdateOnlyRepository
{
    void Update(Entities.User user);
}
