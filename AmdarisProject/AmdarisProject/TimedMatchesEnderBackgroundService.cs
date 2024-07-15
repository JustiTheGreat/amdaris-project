//using AmdarisProject.Application.Abstractions;
//using AmdarisProject.Application.Services;
//using Microsoft.Extensions.Hosting;

//namespace AmdarisProject
//{
//    public class TimedMatchesEnderBackgroundService(IUnitOfWork unitOfWork, IEndMatchService endMatchService) : BackgroundService
//    {
//        private readonly IUnitOfWork _unitOfWork = unitOfWork;
//        private readonly IEndMatchService _endMatchService = endMatchService;

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            Console.WriteLine("a");
//            while (!stoppingToken.IsCancellationRequested)
//            {
//                Console.WriteLine("b");
//                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);

//                IEnumerable<Competition> competitions = _unitOfWork.CompetitionRepository.GetAll().Result;

//                competitions.ToList().ForEach(async competition =>
//                {
//                    Match? firstStartedMatch = _unitOfWork.MatchRepository.GetFirstStartedTimedMatchOfCompetition(competition.Id).Result;

//                    if (firstStartedMatch is null) return;

//                    ulong? matchDuration = competition.GameFormat.DurationInSeconds;

//                    if (matchDuration is null) return;

//                    if (firstStartedMatch.StartTime!.Value.AddSeconds((double)matchDuration) >= DateTime.Now)
//                    {
//                        try
//                        {
//                            await _unitOfWork.BeginTransactionAsync();
//                            await _endMatchService.End(firstStartedMatch.Id, Domain.Enums.MatchStatus.FINISHED);
//                            await _unitOfWork.SaveAsync();
//                            await _unitOfWork.CommitTransactionAsync();

//                            //TODO to remove
//                            Console.WriteLine($"ended match: " +
//                                $"{firstStartedMatch.CompetitorOne.Name}-{firstStartedMatch.CompetitorTwo.Name}");
//                            //
//                        }
//                        catch (Exception)
//                        {
//                            await _unitOfWork.RollbackTransactionAsync();
//                            //TODO to remove
//                            Console.WriteLine("problem");
//                            //
//                        }
//                    }
//                });
//            }
//        }
//    }
//}
