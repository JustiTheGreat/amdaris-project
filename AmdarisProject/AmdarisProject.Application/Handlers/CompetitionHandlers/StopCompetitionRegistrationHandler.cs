using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Application.Handlers.MatchHandlers;
using AmdarisProject.Application.Utils;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using Mapster;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record StopCompetitionRegistration(ulong CompetitionId) : IRequest<CompetitionResponseDTO>;
    public class StopCompetitionRegistrationHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<StopCompetitionRegistration, CompetitionResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<CompetitionResponseDTO> Handle(StopCompetitionRegistration request, CancellationToken cancellationToken)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(nameof(StopCompetitionRegistrationHandler), nameof(Handle),
                    [Tuple.Create(nameof(request.CompetitionId), request.CompetitionId)]);

            if (competition.Status is not CompetitionStatus.ORGANIZING)
                throw new APIllegalStatusException(nameof(StopCompetitionRegistrationHandler), nameof(Handle), competition.Status);

            CheckCompetitionCompetitorNumber(competition);

            Competition updated;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                competition.Status = CompetitionStatus.NOT_STARTED;
                updated = await _unitOfWork.CompetitionRepository.Update(competition);

                //TODO CreateBonusMatches
                await HandlerUtils.CreateCompetitionMatchesUtil(_unitOfWork, updated.Id);

                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            updated = await _unitOfWork.CompetitionRepository.GetById(updated.Id)
                ?? throw new APNotFoundException(nameof(CancelMatchHandler), nameof(Handle),
                    [Tuple.Create(nameof(updated.Id), updated.Id)]);

            //TODO remove
            Console.WriteLine($"Registrations for competition {updated.Name} have stopped!");
            //
            CompetitionResponseDTO response = updated.Adapt<CompetitionResponseDTO>();
            return response;
        }

        private void CheckCompetitionCompetitorNumber(Competition competition)
        {
            void CheckOneVSAllCompetitionCompetitorNumber(OneVSAllCompetition oneVSAllCompetition)
            {
                int competitorNumber = oneVSAllCompetition.Competitors.Count;

                if (competitorNumber < 2)
                    throw new APCompetitorNumberException(nameof(StopCompetitionRegistrationHandler),
                        nameof(CheckOneVSAllCompetitionCompetitorNumber), competitorNumber.ToString());
            }
            void CheckTournamentCompetitionCompetitorNumber(TournamentCompetition tournamentCompetition)
            {
                int competitorNumber = tournamentCompetition.Competitors.Count;

                if (competitorNumber < 2)
                    throw new APCompetitorNumberException(nameof(StopCompetitionRegistrationHandler),
                        nameof(CheckCompetitionCompetitorNumber), competitorNumber.ToString());

                while (competitorNumber != 1)
                {
                    if (competitorNumber % 2 == 1)
                        throw new APCompetitorNumberException(nameof(StopCompetitionRegistrationHandler),
                            nameof(CheckCompetitionCompetitorNumber), competitorNumber.ToString());

                    competitorNumber /= 2;
                }
            }

            if (competition is OneVSAllCompetition oneVSAllCompetition)
                CheckOneVSAllCompetitionCompetitorNumber(oneVSAllCompetition);
            else if (competition is TournamentCompetition tournamentCompetition)
                CheckTournamentCompetitionCompetitorNumber(tournamentCompetition);
        }
    }
}
