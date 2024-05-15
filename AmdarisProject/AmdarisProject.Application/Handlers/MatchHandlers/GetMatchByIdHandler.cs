using AmdarisProject.Application.Common.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.MatchHandlers
{
    public record GetMatchById(Guid MatchId) : IRequest<MatchGetDTO>;
    public class GetMatchByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetMatchByIdHandler> logger)
        : IRequestHandler<GetMatchById, MatchGetDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetMatchByIdHandler> _logger = logger;

        public async Task<MatchGetDTO> Handle(GetMatchById request, CancellationToken cancellationToken)
        {
            Match match = await _unitOfWork.MatchRepository.GetById(request.MatchId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.MatchId), request.MatchId));

            _logger.LogInformation("Got match {CompetitorOneName}-{CompetitorTwoName}!",
                [match.CompetitorOne.Name, match.CompetitorTwo.Name]);

            MatchGetDTO response = _mapper.Map<MatchGetDTO>(match);
            return response;
        }
    }
}
