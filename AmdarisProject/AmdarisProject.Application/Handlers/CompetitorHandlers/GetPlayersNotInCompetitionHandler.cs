using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Common.Models;
using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.GetDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetPlayersNotInCompetition(Guid CompetitionId, PagedRequest PagedRequest)
        : IRequest<PaginatedResult<CompetitorDisplayDTO>>;
    public class GetPlayersNotInCompetitionHandler(IUnitOfWork unitOfWork, IMapper mapper,
        ILogger<GetPlayersNotInCompetitionHandler> logger)
        : IRequestHandler<GetPlayersNotInCompetition, PaginatedResult<CompetitorDisplayDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetPlayersNotInCompetitionHandler> _logger = logger;

        public async Task<PaginatedResult<CompetitorDisplayDTO>> Handle(GetPlayersNotInCompetition request, CancellationToken cancellationToken)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitionId), request.CompetitionId));

            Tuple<IEnumerable<Player>, int> players =
                await _unitOfWork.CompetitorRepository.GetPlayersNotInCompetition(request.CompetitionId, request.PagedRequest);
            IEnumerable<CompetitorDisplayDTO> mapped = _mapper.Map<IEnumerable<CompetitorDisplayDTO>>(players.Item1);
            PaginatedResult<CompetitorDisplayDTO> response = new()
            {
                Items = mapped,
                PageSize = request.PagedRequest.PageSize,
                PageIndex = request.PagedRequest.PageIndex,
                Total = players.Item2
            };

            _logger.LogInformation("Got players not in competition {CompetitionName} (Count = {Count})!",
                [competition.Name, response.Items.Count()]);

            return response;
        }
    }
}
