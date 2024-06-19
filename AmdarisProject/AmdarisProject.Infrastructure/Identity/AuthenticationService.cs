using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.RequestDTOs;
using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.Presentation;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AmdarisProject.Infrastructure.Identity
{
    internal class AuthenticationService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
        ITokenService tokenService, IUnitOfWork unitOfWork, IMapper mapper, ILogger<IAuthenticationService> logger)
        : IAuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly SignInManager<IdentityUser> _signInManager = signInManager;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<IAuthenticationService> _logger = logger;

        public async Task<string> Register(UserRegisterDTO userRegisterDTO)
        {
            if (await _userManager.FindByEmailAsync(userRegisterDTO.Email) is not null)
                throw new APConflictException("Email already in use!");

            if (await _userManager.FindByNameAsync(userRegisterDTO.Username) is not null)
                throw new APConflictException("Username already in use!");

            var identity = new IdentityUser()
            {
                Email = userRegisterDTO.Email,
                UserName = userRegisterDTO.Username
            };

            var player = _mapper.Map<Player>(new CompetitorCreateDTO() { Name = userRegisterDTO.Username });
            await _unitOfWork.CompetitorRepository.Create(player);

            _logger.LogInformation("Created player {PlayerName}!", [player.Name]);

            await _userManager.CreateAsync(identity, userRegisterDTO.Password);

            _logger.LogInformation("Created user {Username}!", [userRegisterDTO.Username]);

            var claims = new List<Claim>() {
                new(ClaimIndetifiers.FirstName, userRegisterDTO.FirstName),
                new(ClaimIndetifiers.LastName, userRegisterDTO.LastName),
                new(ClaimIndetifiers.PlayerId, player.Id.ToString())
            };

            await _userManager.AddClaimsAsync(identity, claims);

            var roleName = nameof(UserRole.User);
            await _userManager.AddToRoleAsync(identity, roleName);

            _logger.LogInformation("User {Username} registered successfully!", [userRegisterDTO.Username]);

            claims.Add(new(ClaimTypes.Role, roleName));

            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new(JwtRegisteredClaimNames.Email, identity.Email));
            claimsIdentity.AddClaim(claims[2]);
            claimsIdentity.AddClaim(claims[3]);

            var token = _tokenService.GenerateAccessToken(claimsIdentity);
            return token;
        }

        public async Task<string> Login(UserLoginDTO userLoginDTO)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(userLoginDTO.Email)
                ?? throw new APConflictException("Incorrect credentials!");

            var result = await _signInManager.CheckPasswordSignInAsync(user, userLoginDTO.Password, false);

            if (!result.Succeeded) throw new APConflictException("Incorrect credentials!");

            _logger.LogInformation("User {username} logged successfully!", [user.UserName]);

            if (user.Email is null) throw new APException("Missing stored email!");

            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new(JwtRegisteredClaimNames.Email, user.Email));
            var claims = await _userManager.GetClaimsAsync(user);
            claimsIdentity.AddClaims(claims);

            var roles = await _userManager.GetRolesAsync(user);
            roles.ToList().ForEach(role => claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role)));

            var token = _tokenService.GenerateAccessToken(claimsIdentity);
            return token;
        }
    }
}
