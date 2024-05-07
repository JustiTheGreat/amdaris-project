using AmdarisProject.Application.Test.ModelBuilder.ModelBuilder;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;
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
    }
}
