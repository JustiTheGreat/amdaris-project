using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.RequestDTOs;
using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.Presentation;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AmdarisProject.Infrastructure.Identity
{
    internal class AuthenticationService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
        SignInManager<IdentityUser> signInManager, ITokenService tokenService, IUnitOfWork unitOfWork, IMapper mapper)
        : IAuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly SignInManager<IdentityUser> _signInManager = signInManager;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<string> Register(UserRegisterDTO userRegisterDTO)
        {
            if (await _userManager.FindByEmailAsync(userRegisterDTO.Email) is not null)
                throw new APConflictException("Email already in use!");

            var identity = new IdentityUser()
            {
                Email = userRegisterDTO.Email,
                UserName = userRegisterDTO.Username
            };

            //tried transactions, but changes needed to be save midway
            var player = _mapper.Map<Player>(new CompetitorCreateDTO() { Name = userRegisterDTO.Username });
            await _unitOfWork.CompetitorRepository.Create(player);

            await _userManager.CreateAsync(identity, userRegisterDTO.Password);
            var claims = new List<Claim>() {
                new(ClaimIndetifiers.FirstName, userRegisterDTO.FirstName),
                new(ClaimIndetifiers.LastName, userRegisterDTO.LastName),
                new(ClaimIndetifiers.PlayerId, player.Id.ToString())
            };

            await _userManager.AddClaimsAsync(identity, claims);

            var roleName = nameof(UserRole.User);
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role is null)
            {
                role = new IdentityRole(roleName);
                await _roleManager.CreateAsync(role);//TODO can't register user
            }

            await _userManager.AddToRoleAsync(identity, roleName);
            claims.Add(new(ClaimTypes.Role, roleName));

            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new(JwtRegisteredClaimNames.Email, identity.Email));
            claimsIdentity.AddClaims(claims);

            var token = _tokenService.GenerateAccessToken(claimsIdentity);
            return token;
        }

        public async Task<string> Login(UserLoginDTO userLoginDTO)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(userLoginDTO.Email)
                ?? throw new APNotFoundException(nameof(userLoginDTO.Email));

            var result = await _signInManager.CheckPasswordSignInAsync(user, userLoginDTO.Password, false);

            if (!result.Succeeded) throw new APUnauthorizedException("Login failed!");

            if (user.Email is null) throw new AmdarisProjectException("Missing stored email!");

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
