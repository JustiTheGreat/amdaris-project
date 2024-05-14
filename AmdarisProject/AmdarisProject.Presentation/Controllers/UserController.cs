using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace AmdarisProject.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(AmdarisProjectDBContext dbContext, UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager, IdentityService identityService)
        : ControllerBase
    {
        private readonly AmdarisProjectDBContext _dbContext = dbContext;
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly SignInManager<IdentityUser> _signInManager = signInManager;
        private readonly IdentityService _identityService = identityService;

        [HttpPost]
        [Route(nameof(Register))]
        public async Task<IActionResult> Register(RegisterUser user)
        {
            var identity = new IdentityUser
            {
                Email = user.Email ?? throw new APArgumentException(nameof(user.Email)),
                UserName = user.Email ?? throw new APArgumentException(nameof(user.Email))
            };
            await _userManager.CreateAsync(identity, user.Password);

            var newClaims = new List<Claim>() {
                new(nameof(user.FirstName), user.FirstName),
                new(nameof(user.LastName), user.LastName)
            };

            await _userManager.AddClaimsAsync(identity, newClaims);

            string roleName = user.Role is UserRole.Administrator ? nameof(UserRole.Administrator) : nameof(UserRole.User);
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role is null)
            {
                role = new IdentityRole(roleName);
                await _roleManager.CreateAsync(role);
            }

            await _userManager.AddToRoleAsync(identity, roleName);
            newClaims.Add(new(ClaimTypes.Role, roleName));

            var claimsIdentity = new ClaimsIdentity(new Claim[]{
                new(JwtRegisteredClaimNames.Sub,identity.Email),
                new(JwtRegisteredClaimNames.Email,identity.Email)
            });

            claimsIdentity.AddClaims(newClaims);

            var token = _identityService.CreateSecurityToken(claimsIdentity);
            var response = _identityService.WriteToken(token);
            return Ok(response);
        }

        [HttpPost]
        [Route(nameof(Login))]
        public async Task<IActionResult> Login(LoginUser user)
        {
            IdentityUser storedUser = await _userManager.FindByEmailAsync(user.Email)
                ?? throw new APArgumentException(nameof(user.Email));

            var result = await _signInManager.CheckPasswordSignInAsync(storedUser, user.Password, false);

            if (!result.Succeeded) throw new APArgumentException(nameof(user.Password));

            if (storedUser.Email is null) throw new APArgumentException(nameof(user.Email));

            var claimsIdentity = new ClaimsIdentity(new Claim[]{
                new(JwtRegisteredClaimNames.Sub,storedUser.Email),
                new(JwtRegisteredClaimNames.Email,storedUser.Email)
            });

            var claims = await _userManager.GetClaimsAsync(storedUser);
            claimsIdentity.AddClaims(claims);

            var roles = await _userManager.GetClaimsAsync(storedUser);
            roles.ToList().ForEach(claimsIdentity.AddClaim);

            var token = _identityService.CreateSecurityToken(claimsIdentity);
            var response = _identityService.WriteToken(token);
            return Ok(response);
        }

        public record RegisterUser(string Email, string Password, string FirstName, string LastName, UserRole Role);
        public record LoginUser(string Email, string Password);
    }
}
