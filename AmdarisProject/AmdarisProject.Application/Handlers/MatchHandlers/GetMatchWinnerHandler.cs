﻿using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Utils;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.Application.Handlers.MatchHandlers
{
    public record GetMatchWinner(ulong MatchId) : IRequest<CompetitorResponseDTO?>;
    public class GetMatchWinnerHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetMatchWinner, CompetitorResponseDTO?>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<CompetitorResponseDTO?> Handle(GetMatchWinner request, CancellationToken cancellationToken)
        {
            Match match = await _unitOfWork.MatchRepository.GetById(request.MatchId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.MatchId), request.MatchId));

            if (match.Status is MatchStatus.NOT_STARTED or MatchStatus.STARTED or MatchStatus.CANCELED)
                throw new APIllegalStatusException(match.Status);

            Competitor? winner;

            if (match.Status is MatchStatus.SPECIAL_WIN_COMPETITOR_ONE)
                winner = match.CompetitorOne;
            else if (match.Status is MatchStatus.SPECIAL_WIN_COMPETITOR_TWO)
                winner = match.CompetitorTwo;
            else
            {
                uint pointsCompetitorOne = await HandlerUtils.GetCompetitorMatchPointsUtil(_unitOfWork, match.Id, match.CompetitorOne.Id);
                uint pointsCompetitorTwo = await HandlerUtils.GetCompetitorMatchPointsUtil(_unitOfWork, match.Id, match.CompetitorTwo.Id);
                winner = pointsCompetitorOne == pointsCompetitorTwo ? null
                    : pointsCompetitorOne > pointsCompetitorTwo ? match.CompetitorOne
                    : match.CompetitorTwo;
            }

            CompetitorResponseDTO? response = winner is null ? null : _mapper.Map<CompetitorResponseDTO>(winner);
            return response;
        }
    }
}
