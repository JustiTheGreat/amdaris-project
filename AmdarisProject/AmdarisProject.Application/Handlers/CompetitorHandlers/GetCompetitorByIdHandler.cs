using AmdarisProject.Application.Common.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetCompetitorById(Guid CompetitorId) : IRequest<CompetitorGetDTO>;
    public class GetCompetitorByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetCompetitorByIdHandler> logger)
        : IRequestHandler<GetCompetitorById, CompetitorGetDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetCompetitorByIdHandler> _logger = logger;

        public async Task<CompetitorGetDTO> Handle(GetCompetitorById request, CancellationToken cancellationToken)
        {
            Competitor competitor = await _unitOfWork.CompetitorRepository.GetById(request.CompetitorId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitorId), request.CompetitorId));

            _logger.LogInformation("Got competitor {CompetitorName}!", [competitor.Name]);

            CompetitorGetDTO response = competitor is Player ? _mapper.Map<PlayerGetDTO>(competitor)
                : competitor is Team ? _mapper.Map<TeamGetDTO>(competitor)
                : throw new AmdarisProjectException(nameof(competitor));
            return response;
        }
    }
}
