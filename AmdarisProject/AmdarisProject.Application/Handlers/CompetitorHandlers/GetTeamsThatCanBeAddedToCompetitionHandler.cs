using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Common.Models;
using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetTeamsThatCanBeAddedToCompetition(Guid CompetitionId, PagedRequest PagedRequest)
        : IRequest<PaginatedResult<CompetitorDisplayDTO>>;
    public class GetTeamsThatCanBeAddedToCompetitionHandler(IUnitOfWork unitOfWork, IMapper mapper,
        ILogger<GetTeamsThatCanBeAddedToCompetitionHandler> logger)
        : IRequestHandler<GetTeamsThatCanBeAddedToCompetition, PaginatedResult<CompetitorDisplayDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetTeamsThatCanBeAddedToCompetitionHandler> _logger = logger;

        public async Task<PaginatedResult<CompetitorDisplayDTO>> Handle(GetTeamsThatCanBeAddedToCompetition request, CancellationToken cancellationToken)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitionId), request.CompetitionId));

            Tuple<IEnumerable<Team>, int> teams = await _unitOfWork.CompetitorRepository
                .GetTeamsThatCanBeAddedToCompetition(request.CompetitionId, (uint)competition.GameFormat.TeamSize!, request.PagedRequest);
            IEnumerable<CompetitorDisplayDTO> mapped = _mapper.Map<IEnumerable<CompetitorDisplayDTO>>(teams.Item1);
            PaginatedResult<CompetitorDisplayDTO> response = new()
            {
                Items = mapped,
                PageSize = request.PagedRequest.PageSize,
                PageIndex = request.PagedRequest.PageIndex,
                Total = teams.Item2
            };

            _logger.LogInformation("Got teams that could be added to competition {CompetitionName} (Count = {Count})!",
                [competition.Name, response.Items.Count()]);

            return response;
        }
    }
}
