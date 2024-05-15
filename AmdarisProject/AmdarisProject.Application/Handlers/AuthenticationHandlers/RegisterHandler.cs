using AmdarisProject.Application.Common.Abstractions;
using AmdarisProject.Application.Dtos.RequestDTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.AuthenticationHandlers
{
    public record Register(UserRegisterDTO UserRegisterDTO) : IRequest<string>;
    public class RegisterHandler(IAuthenticationService authenticationService, ILogger<RegisterHandler> logger)
        : IRequestHandler<Register, string>
    {
        private readonly IAuthenticationService _authenticationService = authenticationService;
        private readonly ILogger<RegisterHandler> _logger = logger;

        public async Task<string> Handle(Register request, CancellationToken cancellationToken)
        {
            string token = await _authenticationService.Register(request.UserRegisterDTO);

            _logger.LogInformation("User with email {Email} registered successfully!", [request.UserRegisterDTO.Email]);

            return token;
        }
    }
}
