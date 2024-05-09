using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Test.ModelBuilders.CompetitorBuilders
{
    public abstract class CompetitiorBuilder<T, Y>(T competitor)
        : ModelBuilder<T, Y>(competitor) where T : Competitor where Y : CompetitiorBuilder<T, Y>
    {
        public Y SetName(string name)
        {
            _model.Name = name;
            return (Y)this;
        }

        public Y SetMatches(List<Match> matches)
        {
            _model.Matches = matches;
            return (Y)this;
        }

        public Y SetWonMatches(List<Match> matches)
        {
            _model.WonMatches = matches;
            return (Y)this;
        }

        public Y SetCompetitions(List<Competition> competitions)
        {
            _model.Competitions = competitions;
            return (Y)this;
        }
    }
}
