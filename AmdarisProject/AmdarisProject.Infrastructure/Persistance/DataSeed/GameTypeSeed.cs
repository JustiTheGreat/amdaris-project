using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.Infrastructure.Persistance.Contexts;
using System.Security.AccessControl;

namespace AmdarisProject.Infrastructure.Persistance.DataSeed
{
    internal class GameTypeSeed
    {
        public static async Task Seed(AmdarisProjectDBContext dbContext)
        {
            List<GameType> gameTypes = new List<string> { "Ping pong", "Chess", "FIFA", "Football", "Basketball", "Tenis", "Hockey", "Rugby" }
                .Select(gameTypeName => new GameType() { Name = gameTypeName }).ToList();

            List<Player> players = new List<string>
            {
                "QuantumDreamer",
                "PixelPioneer",
                "NebulaNavigator",
                "StellarWhisper",
                "LunarVoyager",
                "MysticEchoes",
                "CyberSorcerer",
                "EchoKnight",
                "FrostyPhoenix",
                "BlazeGuardian",
                "VelvetSpecter",
                "TitanTrail"
            }
            .Select(username => new Player()
            {
                Name = username,
                Matches = [],
                WonMatches = [],
                Competitions = [],
                TeamPlayers = [],
                Points = [],
                Teams = [],
                ProfilePictureUri = null,
            }).ToList();

            List<Team> teams = new List<string>
            {
                "Thunderbolts",
                "Crimson Titans",
                "Mystic Warriors",
                "Solar Sentinels",
                "Shadow Strikers",
                "Ironclad Guardians"
            }
            .Select(teamName => new Team()
            {
                Name = teamName,
                Matches = [],
                WonMatches = [],
                Competitions = [],
                TeamPlayers = [],
                Players = [],
            }).ToList();

            List<GameFormat> gameFormats = new List<GameFormat>
            {
                new GameFormat()
                {
                    Name="PingPongSingle",
                    CompetitorType=Domain.Enums.CompetitorType.PLAYER,
                    GameType=gameTypes[0],
                    TeamSize=null,
                    WinAt=21,
                    DurationInMinutes=null,
                },
                new GameFormat()
                {
                    Name="PingPongTeam",
                    CompetitorType=Domain.Enums.CompetitorType.TEAM,
                    GameType=gameTypes[0],
                    TeamSize=2,
                    WinAt=21,
                    DurationInMinutes=null,
                },
                new GameFormat()
                {
                    Name="Fifa",
                    CompetitorType=Domain.Enums.CompetitorType.PLAYER,
                    GameType=gameTypes[2],
                    TeamSize=null,
                    WinAt=null,
                    DurationInMinutes=90,
                },
                new GameFormat()
                {
                    Name="Chess",
                    CompetitorType=Domain.Enums.CompetitorType.PLAYER,
                    GameType=gameTypes[1],
                    TeamSize=null,
                    WinAt=1,
                    DurationInMinutes=null,
                },
                new GameFormat()
                {
                    Name="Football",
                    CompetitorType=Domain.Enums.CompetitorType.TEAM,
                    GameType=gameTypes[3],
                    TeamSize=5,
                    WinAt=null,
                    DurationInMinutes=30,
                },
                 new GameFormat()
                {
                    Name="Basketball",
                    CompetitorType=Domain.Enums.CompetitorType.TEAM,
                    GameType=gameTypes[3],
                    TeamSize=3,
                    WinAt=null,
                    DurationInMinutes=15,
                },
            };

            gameTypes.ForEach((o) => dbContext.AddAsync(o));
            players.ForEach(o => dbContext.AddAsync(o));
            teams.ForEach(o => dbContext.AddAsync(o));
            gameFormats.ForEach(o => dbContext.AddAsync(o));
            await dbContext.SaveChangesAsync();
        }
    }
}
