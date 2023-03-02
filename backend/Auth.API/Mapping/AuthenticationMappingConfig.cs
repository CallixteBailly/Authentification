using Auth.Contracts.Authentication;
using Mapster;
using Auth.Common;
using Auth.API.Application.Commands;
using Auth.Application.Queries.Login;
using Auth.Application.Queries.VerifyToken;

namespace Auth.Api.Common.Mapping
{
    public class AuthenticationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RegisterRequest, RegisterCommand>();
            config.NewConfig<LoginRequest, LoginQuery>();
            config.NewConfig<AuthenticationResult, AuthenticationResponse>()
                .Map(target => target, source => source.User)
                .Map(target => target.Token, source => source.Token);
            config.ForType<VerifyTokenRequest, VerifyTokenQuery>();
        }
    }
}