﻿using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Application.Utils;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using Mapster;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitionHandlers
{
    public record AddCompetitorToCompetition(ulong CompetitorId, ulong CompetitionId) : IRequest<CompetitionResponseDTO>;
    public class AddCompetitorToCompetitionHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<AddCompetitorToCompetition, CompetitionResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<CompetitionResponseDTO> Handle(AddCompetitorToCompetition request, CancellationToken cancellationToken)
        {
            Competitor competitor = await _unitOfWork.CompetitorRepository.GetById(request.CompetitorId)
                    ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitorId), request.CompetitorId));

            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitionId), request.CompetitionId));

            if (competition.Status is not CompetitionStatus.ORGANIZING)
                throw new APIllegalStatusException(competition.Status);

            if (HandlerUtils.CompetitionContainsCompetitor(competition, request.CompetitorId))
                throw new AmdarisProjectException($"Competitor {competitor.Id} is already registered to {competition.Id}!");

            //TODO check if competitor is team and player from team is in another team from competition

            Competition updated;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                competition.Competitors = [.. competition.Competitors, competitor];
                updated = await _unitOfWork.CompetitionRepository.Update(competition);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            //TODO remove
            Console.WriteLine($"Competitor {competitor.Name} has registered to competition {competition.Name}!");
            //

            CompetitionResponseDTO response = competition is OneVSAllCompetition ? competition.Adapt<OneVSAllCompetitionResponseDTO>()
                : competition is TournamentCompetition ? competition.Adapt<TournamentCompetitionResponseDTO>()
                : throw new AmdarisProjectException(nameof(competition));

            return response;
        }
    }
}
