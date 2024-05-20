using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.RequestDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Presentation;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AmdarisProject.Infrastructure.Identity
{
    internal class AuthenticationService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
        SignInManager<IdentityUser> signInManager, ITokenService tokenService)
        : IAuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly SignInManager<IdentityUser> _signInManager = signInManager;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<string> Register(UserRegisterDTO userRegisterDTO)
        {
            if (await _userManager.FindByEmailAsync(userRegisterDTO.Email) is not null)
                throw new APConflictException("Email already in use!");

            var identity = new IdentityUser
            {
                Email = userRegisterDTO.Email,
                UserName = userRegisterDTO.Email
            };

            await _userManager.CreateAsync(identity, userRegisterDTO.Password);

            var claims = new List<Claim>() {
                new(nameof(userRegisterDTO.FirstName), userRegisterDTO.FirstName),
                new(nameof(userRegisterDTO.LastName), userRegisterDTO.LastName)
            };

            await _userManager.AddClaimsAsync(identity, claims);

            string roleName = userRegisterDTO.Role is UserRole.Administrator
                ? nameof(UserRole.Administrator)
                : nameof(UserRole.User);
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role is null)
            {
                role = new IdentityRole(roleName);
                await _roleManager.CreateAsync(role);
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
