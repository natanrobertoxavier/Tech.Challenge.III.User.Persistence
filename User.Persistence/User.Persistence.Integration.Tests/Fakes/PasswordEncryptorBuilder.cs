using TokenService.Manager.Controller;

namespace User.Persistence.Integration.Tests.Fakes;
public class PasswordEncryptorBuilder
{
    public static PasswordEncryptor Build()
    {
        return new PasswordEncryptor("%xIQ*83Y0K!@");
    }
}
