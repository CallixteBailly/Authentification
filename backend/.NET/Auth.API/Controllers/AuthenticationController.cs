using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Auth.Contracts.Authentication;
using Auth.API.Application.Commands;
using Auth.Application.Queries.Login;
using Auth.Application.Queries.VerifyToken;

namespace Auth.Api.Controllers
{
    [Route("authentication")]
    [AllowAnonymous]
    public class AuthenticationController : ApiController
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public AuthenticationController(
            ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var command = _mapper.Map<RegisterCommand>(request);

            var authResult = await _mediator.Send(command);

            return authResult.Match(
                authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
                errors => Problem(errors)
            );
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var query = _mapper.Map<LoginQuery>(request);

            var authResult = await _mediator.Send(query);

            return authResult.Match(
                authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
                errors => Problem(errors)
            );
        }

        [HttpPost("verifyToken")]
        public async Task<IActionResult> VerifyToken(VerifyTokenRequest request)
        {
            var query = _mapper.Map<VerifyTokenQuery>(request);

            var authResult = await _mediator.Send(query);

            return authResult.Match(
                authResult => Ok(_mapper.Map<bool>(authResult)),
                errors => Problem(errors)
            );
        }
    }
}