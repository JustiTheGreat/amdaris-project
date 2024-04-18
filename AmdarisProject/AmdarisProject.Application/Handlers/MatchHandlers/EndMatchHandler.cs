using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Services;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.Application.Handlers.MatchHandlers
{
    public record EndMatch(Guid MatchId, MatchStatus Status) : IRequest<MatchResponseDTO>;
    public class EndMatchHandler(IUnitOfWork unitOfWork, IMapper mapper, IEndMatchService endMatchService)
        : IRequestHandler<EndMatch, MatchResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IEndMatchService _endMatchService = endMatchService;

        public async Task<MatchResponseDTO> Handle(EndMatch request, CancellationToken cancellationToken)
        {
            Match updated = await _endMatchService.End(request.MatchId, request.Status);
            MatchResponseDTO response = _mapper.Map<MatchResponseDTO>(updated);
            return response;
        }
    }
}
