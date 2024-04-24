﻿using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetPlayersNotInTeam(Guid TeamId) : IRequest<IEnumerable<PlayerDisplayDTO>>;
    public class GetPlayersNotInTeamHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetPlayersNotInTeam, IEnumerable<PlayerDisplayDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<PlayerDisplayDTO>> Handle(GetPlayersNotInTeam request, CancellationToken cancellationToken)
        {
            IEnumerable<Player> players = await _unitOfWork.CompetitorRepository.GetPlayersNotInTeam(request.TeamId);
            IEnumerable<PlayerDisplayDTO> response = _mapper.Map<List<PlayerDisplayDTO>>(players);
            return response;
        }
    }
}
