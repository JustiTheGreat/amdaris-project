using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net;
using AmdarisProject.Infrastructure.Identity;
using Microsoft.Extensions.Options;
using AmdarisProject.Application.Options;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp;
using AmdarisProject.Application.Abstractions;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.Domain.Extensions;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record SendDiplomaToPlayers(Guid CompetitionId) : IRequest<bool>;
    public class SendDiplomaToPlayersHandler(IUnitOfWork unitOfWork, ICompetitionRankingService competitionRankingService,
        UserManager<IdentityUser> userManager, IOptions<SmtpSettings> smtpSettings)
        : IRequestHandler<SendDiplomaToPlayers, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICompetitionRankingService _competitionRankingService = competitionRankingService;
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly SmtpSettings _smtpSettings = smtpSettings.Value;

        public async Task<bool> Handle(SendDiplomaToPlayers request, CancellationToken cancellationToken)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitionId), request.CompetitionId));

            IEnumerable<Competitor> winners = await _competitionRankingService.GetCompetitionWinners(request.CompetitionId);
            IEnumerable<string> playerIds =
                competition.GameFormat.CompetitorType is Domain.Enums.CompetitorType.PLAYER
                ? winners.GetIds().Select(id => id.ToString()).ToList()
                : competition.GameFormat.CompetitorType is Domain.Enums.CompetitorType.TEAM
                ? winners
                    .Aggregate(new List<Player>(), (result, team) => { result.AddRange(((Team)team).Players); return result; })
                    .Select(player => player.Id.ToString()).ToList()
                : throw new APException("Unexpected competitor type!");

            IEnumerable<IdentityUser> users = _userManager.Users.ToList();
            IEnumerable<UserData> userDataList = (IEnumerable<UserData>)users
                .Select(user =>
                {
                    var userClaims = _userManager.GetClaimsAsync(user).Result;
                    var playerId = userClaims
                        .FirstOrDefault(claim =>
                            claim.Type.Equals(ClaimIndetifiers.PlayerId)
                            && playerIds.Contains(claim.Value))?
                        .Value;

                    if (playerId is null) return null;

                    return new UserData()
                    {
                        Email = user.Email,
                        FirstName = userClaims.FirstOrDefault(claim => claim.Type.Equals(ClaimIndetifiers.FirstName))?.Value,
                        LastName = userClaims.FirstOrDefault(claim => claim.Type.Equals(ClaimIndetifiers.LastName))?.Value,
                        Username = user.UserName,
                    };
                })
                .Where(userData => userData is not null)
                .ToList();

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = _smtpSettings.Port,
                Credentials = new NetworkCredential(_smtpSettings.Email, _smtpSettings.Password),
                EnableSsl = true,
            };

            foreach (UserData userData in userDataList)
            {
                if (userData.Email is null) continue;

                MailMessage mail = new();
                mail.From = new MailAddress(_smtpSettings.Email);
                mail.To.Add(userData.Email);
                mail.Subject = "Congratulations! Here's your diploma";
                mail.Body = $"Dear {userData.LastName} {userData.FirstName},\n\n" +
                    $"Congratulations on winning the competition! Attached is your diploma.\n\n" +
                    $"Best regards,\n" +
                    $"ContestCraft";
                MemoryStream diplomaStream = CreateDiploma(userData.FirstName, userData.LastName, userData.Username,
                    competition.Name, competition.ActualizedStartTime.ToString().Split(" ")[0]);
                diplomaStream.Seek(0, SeekOrigin.Begin);
                Attachment attachment = new(diplomaStream, "Diploma.pdf", "application/pdf");
                mail.Attachments.Add(attachment);

                smtpClient.Send(mail);
            }

            return true;
        }

        private static MemoryStream CreateDiploma(string firstName, string lastName, string userName, string competitionName,
            string competitionStartTime)
        {
            PdfDocument document = new();
            document.Info.Title = "Diploma";

            PdfPage page = document.AddPage();
            page.Orientation = PageOrientation.Landscape;
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont titleFont = new("Arial", 36, XFontStyleEx.Bold);
            XFont textFont = new("Arial", 18);

            gfx.DrawString("Winner diploma", titleFont, XBrushes.Black, new XRect(0, 100, page.Width, 50), XStringFormats.Center);

            //string diplomaText = $"This diploma is accorded to {lastName} {firstName} as the player {userName},\n" +
            //    $" for being the winner of the competition {competitionName}\n" +
            //    $" held on the date of {competitionStartTime}!";

            gfx.DrawString($"This diploma is accorded to {lastName} {firstName} as the player \"{userName}\",", textFont, XBrushes.Black, new XRect(0, 200, page.Width, 20), XStringFormats.Center);
            gfx.DrawString($" for being the winner of the competition \"{competitionName}\"", textFont, XBrushes.Black, new XRect(0, 220, page.Width, 20), XStringFormats.Center);
            gfx.DrawString($" held on the date of {competitionStartTime}!", textFont, XBrushes.Black, new XRect(0, 240, page.Width, 20), XStringFormats.Center);

            MemoryStream stream = new();
            document.Save(stream, false);

            return stream;
        }

        private class UserData
        {
            public string? Email { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? Username { get; set; }
        }
    }
}
