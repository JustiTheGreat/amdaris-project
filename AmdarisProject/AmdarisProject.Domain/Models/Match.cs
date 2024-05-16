using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Domain.Models
{
    public class Match : Model
    {
        public required string Location { get; set; }
        public required DateTime? StartTime { get; set; }
        public required DateTime? EndTime { get; set; }
        public required MatchStatus Status { get; set; }
        public virtual required Competitor CompetitorOne { get; set; }
        public virtual required Competitor CompetitorTwo { get; set; }
        public virtual required Competition Competition { get; set; }
        public required uint? CompetitorOnePoints { get; set; }
        public required uint? CompetitorTwoPoints { get; set; }
        public virtual required Competitor? Winner { get; set; }
        public required uint? StageLevel { get; set; }
        public required uint? StageIndex { get; set; }
        public virtual required List<Point> Points { get; set; } = [];

        public Competitor? GetWinner()
            => Status is MatchStatus.SPECIAL_WIN_COMPETITOR_ONE ? CompetitorOne
            : Status is MatchStatus.SPECIAL_WIN_COMPETITOR_TWO ? CompetitorTwo
            : Status is MatchStatus.FINISHED ?
                CompetitorOnePoints > CompetitorTwoPoints ? CompetitorOne
                : CompetitorOnePoints < CompetitorTwoPoints ? CompetitorTwo
                : null
            : throw new APIllegalStatusException(Status);
    }
}
