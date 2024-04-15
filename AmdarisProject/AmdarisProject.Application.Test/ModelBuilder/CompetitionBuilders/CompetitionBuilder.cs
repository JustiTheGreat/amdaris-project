using AmdarisProject.Application.Test.ModelBuilder.ModelBuilder;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models.CompetitionModels;

namespace AmdarisProject.Application.Test.ModelBuilder.CompetitionBuilders
{
    internal abstract class CompetitionBuilder<T, Y>(T competition)
        : ModelBuilder<T, Y>(competition) where T : Competition where Y : CompetitionBuilder<T, Y>
    {
        public Y SetStatus(CompetitionStatus competitionStatus)
        {
            _model.Status = competitionStatus;
            return (Y)this;
        }

        public Y SetWinAt(ushort? winAt)
        {
            _model.WinAt = winAt;
            return (Y)this;
        }

        public Y SetDurationInSeconds(ulong? durationInSeconds)
        {
            _model.DurationInSeconds = durationInSeconds;
            return (Y)this;
        }

        public Y SetBreakInSeconds(ulong? breakInSeconds)
        {
            _model.BreakInSeconds = breakInSeconds;
            return (Y)this;
        }

        public Y SetCompetitorType(CompetitorType competitorType)
        {
            _model.CompetitorType = competitorType;
            return (Y)this;
        }

        public Y SetTeamSize(ushort teamSize)
        {
            _model.TeamSize = teamSize;
            return (Y)this;
        }
    }
}
