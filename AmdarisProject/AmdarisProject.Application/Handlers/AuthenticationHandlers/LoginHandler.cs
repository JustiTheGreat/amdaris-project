using AmdarisProject.Application.Common.Abstractions;
using AmdarisProject.Application.Dtos.RequestDTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.AuthenticationHandlers
{
    public record Login(UserLoginDTO UserLoginDTO) : IRequest<string>;
    public class LoginHandler(IAuthenticationService authenticationService, ILogger<LoginHandler> logger)
        : IRequestHandler<Login, string>
    {
        private readonly IAuthenticationService _authenticationService = authenticationService;
        private readonly ILogger<LoginHandler> _logger = logger;

        public async Task<string> Handle(Login request, CancellationToken cancellationToken)
        {
            string token = await _authenticationService.Login(request.UserLoginDTO);

            _logger.LogInformation("User with email {Email} logged successfully!", [request.UserLoginDTO.Email]);

            return token;
        }
    }
}
