using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.RequestDTOs;
using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.Presentation;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Numerics;
using System.Security.Claims;
using System.Security.Principal;

namespace AmdarisProject.Infrastructure.Identity
{
    internal class AuthenticationService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
        ITokenService tokenService, IUnitOfWork unitOfWork, IMapper mapper, ILogger<IAuthenticationService> logger,
        IBlobStorageService blobStorageService)
        : IAuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly SignInManager<IdentityUser> _signInManager = signInManager;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<IAuthenticationService> _logger = logger;
        private readonly IBlobStorageService _blobStorageService = blobStorageService;

        public async Task<string> Register(UserRegisterDTO userRegisterDTO)
        {
            if (await _userManager.FindByEmailAsync(userRegisterDTO.Email) is not null)
                throw new APConflictException("Email already in use!");

            if (await _userManager.FindByNameAsync(userRegisterDTO.Username) is not null)
                throw new APConflictException("Username already in use!");

            var user = new IdentityUser()
            {
                Email = userRegisterDTO.Email,
                UserName = userRegisterDTO.Username
            };

            Player player = _mapper.Map<Player>(new CompetitorCreateDTO() { Name = userRegisterDTO.Username, ProfilePictureUri = null });
            player.ProfilePictureUri = userRegisterDTO.ProfilePicture is not null
                ? await _blobStorageService.UploadFile(userRegisterDTO.ProfilePicture)
                : null;
            await _unitOfWork.CompetitorRepository.Create(player);

            _logger.LogInformation("Created player {PlayerName}!", [player.Name]);

            await _userManager.CreateAsync(user, userRegisterDTO.Password);

            _logger.LogInformation("Created user {Username}!", [userRegisterDTO.Username]);

            var claims = new List<Claim>() {
                new(ClaimIndetifiers.FirstName, userRegisterDTO.FirstName),
                new(ClaimIndetifiers.LastName, userRegisterDTO.LastName),
                new(ClaimIndetifiers.PlayerId, player.Id.ToString())
            };

            if (player.ProfilePictureUri is not null)
                claims.Add(new(ClaimIndetifiers.ProfilePictureUri, player.ProfilePictureUri));

            await _userManager.AddClaimsAsync(user, claims);

            var roleName = nameof(UserRole.User);
            await _userManager.AddToRoleAsync(user, roleName);

            _logger.LogInformation("User {Username} registered successfully!", [userRegisterDTO.Username]);

            claims.Add(new(ClaimTypes.Role, roleName));

            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new(ClaimIndetifiers.Username, user.UserName));
            claimsIdentity.AddClaim(new(ClaimIndetifiers.Email, user.Email));
            claimsIdentity.AddClaims(claims);

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

            if (user.UserName is null) throw new APException("Missing stored username!");

            if (user.Email is null) throw new APException("Missing stored email!");

            var claimsIdentity = new ClaimsIdentity();
            var claims = await _userManager.GetClaimsAsync(user);
            claimsIdentity.AddClaims(claims);
            claimsIdentity.AddClaim(new(ClaimIndetifiers.Username, user.UserName));
            claimsIdentity.AddClaim(new(ClaimIndetifiers.Email, user.Email));

            var roles = await _userManager.GetRolesAsync(user);
            roles.ToList().ForEach(role => claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role)));

            var token = _tokenService.GenerateAccessToken(claimsIdentity);
            return token;
        }

        public async Task<string> UpdateProfile(string email, UpdateProfileDTO updateProfileDTO)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(email)
                ?? throw new APException("User not found!");

            IdentityUser? userByUsername = await _userManager.FindByNameAsync(updateProfileDTO.Username);

            if (userByUsername is not null && userByUsername.UserName!.Equals(user.UserName) && !userByUsername.Email!.Equals(user.Email))
                throw new APConflictException("Username already in use!");

            user.UserName = updateProfileDTO.Username;
            await _userManager.UpdateAsync(user);

            var userClaims = await _userManager.GetClaimsAsync(user);
            Claim? oldProfilePictureUriClaim = userClaims.FirstOrDefault(claim => claim.Type.ToString().Equals(ClaimIndetifiers.ProfilePictureUri));

            if (oldProfilePictureUriClaim is not null || updateProfileDTO.ProfilePicture is not null)
            {
                if (oldProfilePictureUriClaim is not null)
                    await _userManager.RemoveClaimAsync(user, oldProfilePictureUriClaim);

                string? newProfilePictureUri = null;

                if (oldProfilePictureUriClaim is null)
                    newProfilePictureUri = await _blobStorageService.UploadFile(updateProfileDTO.ProfilePicture!);
                else if (oldProfilePictureUriClaim is not null && updateProfileDTO.ProfilePicture is not null)
                    newProfilePictureUri =
                        await _blobStorageService.UpdateFile(oldProfilePictureUriClaim.Value.ToString().Split("/").Last(), updateProfileDTO.ProfilePicture!);
                else await _blobStorageService.DeleteFile(oldProfilePictureUriClaim!.Value.ToString().Split("/").Last());


                if (newProfilePictureUri is not null)
                    await _userManager.AddClaimAsync(user, new Claim(ClaimIndetifiers.ProfilePictureUri, newProfilePictureUri));

                string playerId = userClaims.FirstOrDefault(claim => claim.Type.ToString().Equals(ClaimIndetifiers.PlayerId))?.Value
                    ?? throw new APException("PlayerId not found!");

                Player player = await _unitOfWork.CompetitorRepository.GetById(Guid.Parse(playerId)) as Player
                    ?? throw new APNotFoundException(playerId);

                player.ProfilePictureUri = newProfilePictureUri;
                await _unitOfWork.CompetitorRepository.Update(player);
                await _unitOfWork.SaveAsync();
            }

            var claimsIdentity = new ClaimsIdentity();
            var claims = await _userManager.GetClaimsAsync(user);
            claimsIdentity.AddClaims(claims);
            claimsIdentity.AddClaim(new(ClaimIndetifiers.Username, user.UserName));
            claimsIdentity.AddClaim(new(ClaimIndetifiers.Email, user.Email!));
            var roles = await _userManager.GetRolesAsync(user);
            roles.ToList().ForEach(role => claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role)));

            var token = _tokenService.GenerateAccessToken(claimsIdentity);
            return token;
        }
    }
}
