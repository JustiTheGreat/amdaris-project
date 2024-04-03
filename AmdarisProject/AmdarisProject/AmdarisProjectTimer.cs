namespace AmdarisProject
{
    public class AmdarisProjectTimer
    {
        //private static System.Timers.Timer _timer;
        //private static readonly List<Competition> _supervisedCompetitions = [];

        //public static void AddCompetitionToSupervise(Competition competition)
        //{
        //    _supervisedCompetitions.Add(competition);
        //}

        //public static void SetTimer()
        //{
        //    _timer = new(1);
        //    _timer.Elapsed += OnTimedEvent;
        //    _timer.AutoReset = true;
        //    _timer.Enabled = true;
        //}

        //private static void OnTimedEvent(object source, ElapsedEventArgs e)
        //{
        //    _supervisedCompetitions.Where(competition => competition.GameRules.DurationInSeconds is not null)
        //        .Aggregate(new List<Match>(), (matches, competition) =>
        //        {
        //            matches.AddRange(competition.Matches);
        //            return matches;
        //        })
        //        .Where(match => match.Status == utils.enums.MatchStatus.STARTED)
        //        .ToList()
        //        .ForEach(match =>
        //        {
        //            long matchDuration = match.Competition.GameRules.DurationInSeconds
        //                ?? throw new AmdarisProjectException(MessageFormatter.Format(nameof(AmdarisProjectTimer), nameof(OnTimedEvent),
        //                    "null match duration"));

        //            if (match.StartTime.Value.AddSeconds(matchDuration) >= DateTime.Now)
        //                match.End();
        //        });
        //}
    }
}
