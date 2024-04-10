using AmdarisProject.Domain.Models.CompetitionModels;

namespace AmdarisProject.Domain.Models
{
    public class Stage : Model
    {
        public ushort StageLevel { get; set; }
        public List<Match> Matches { get; set; } = [];
        public TournamentCompetition TournamentCompetition { get; set; }

        public Stage() { }

        public Stage(ushort stageLevel, List<Match> matches, TournamentCompetition tournamentCompetition)
        {
            StageLevel = stageLevel;
            Matches = matches;
            TournamentCompetition = tournamentCompetition;
        }
    }
}
