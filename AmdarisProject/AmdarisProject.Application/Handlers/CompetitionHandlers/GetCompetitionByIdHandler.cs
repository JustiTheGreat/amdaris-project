using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.handlers.competition
{
    public record GetCompetitionById(Guid CompetitionId) : IRequest<CompetitionGetDTO>;
    public class GetCompetitionByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetCompetitionByIdHandler> logger)
        : IRequestHandler<GetCompetitionById, CompetitionGetDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetCompetitionByIdHandler> _logger = logger;

        public async Task<CompetitionGetDTO> Handle(GetCompetitionById request, CancellationToken cancellationToken)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitionId), request.CompetitionId));

            _logger.LogInformation("Got competition {CompetitorName}!", [competition.Name]);

            CompetitionGetDTO response = competition is OneVSAllCompetition ? _mapper.Map<OneVSAllCompetitionGetDTO>(competition)
                : competition is TournamentCompetition ? _mapper.Map<TournamentCompetitionGetDTO>(competition)
                : throw new AmdarisProjectException("Unexpected competition type!");
            return response;
        }
    }
}
