using AutoMapper;

namespace User.Persistence.Api.Automapper;

public class TechChallengeProfile : Profile
{
    public TechChallengeProfile()
    {
        RequestToEntity();
        EntityToResponse();
    }

    private static void EntityToResponse()
    {
    }

    private void RequestToEntity()
    {
        CreateMap<Communication.Request.RequestRegisterUserJson, Domain.Entities.User>()
           .ForMember(destiny => destiny.Password, config => config.Ignore());
    }
}
