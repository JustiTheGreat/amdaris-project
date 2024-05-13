using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Test.ModelBuilders.CompetitionBuilders
{
    public abstract class CompetitionBuilder<T, Y>(T competition)
        : ModelBuilder<T, Y>(competition) where T : Competition where Y : CompetitionBuilder<T, Y>
    {
        public Y SetName(string name)
        {
            _model.Name = name;
            return (Y)this;
        }

        public Y SetLocation(string location)
        {
            _model.Location = location;
            return (Y)this;
        }

        public Y SetStartTime(DateTime startTime)
        {
            _model.StartTime = startTime;
            return (Y)this;
        }

        public Y SetStatus(CompetitionStatus competitionStatus)
        {
            _model.Status = competitionStatus;
            return (Y)this;
        }

        public Y SetBreakInMinutes(ulong? breakInMinutes)
        {
            _model.BreakInMinutes = breakInMinutes;
            return (Y)this;
        }

        public Y SetGameFormat(GameFormat gameFormat)
        {
            _model.GameFormat = gameFormat;
            return (Y)this;
        }

        public Y SetCompetitors(List<Competitor> competitors)
        {
            _model.Competitors = competitors;
            return (Y)this;
        }

        public Y SetMatches(List<Match> matches)
        {
            _model.Matches = matches;
            return (Y)this;
        }

        public Y AddCompetitor(Competitor competitor)
        {
            _model.Competitors.Add(competitor);
            competitor.Competitions.Add(_model);
            return (Y)this;
        }
    }
}
