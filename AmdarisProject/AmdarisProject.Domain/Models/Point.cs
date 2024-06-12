using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Domain.Models
{
    public class Point : Model
    {
        public required uint Value { get; set; }
        public virtual required Match Match { get; set; }
        public virtual required Player Player { get; set; }

        public void AddValue(uint value)
        {
            if (Match.Status is not MatchStatus.STARTED)
                throw new APIllegalStatusException(Match.Status);

            if (Match.ACompetitorHasTheWinningScore())
                throw new APException($"A competitor of match {Match.CompetitorOne.Name}-{Match.CompetitorTwo.Name} already has the winning number of points!");

            Value += value;

            Match.AddPointsForPlayerSide(Player.Id, value);
        }
    }
}
