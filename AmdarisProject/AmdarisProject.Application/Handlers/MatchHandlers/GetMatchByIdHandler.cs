using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.Application.Handlers.MatchHandlers
{
    public record GetMatchById(Guid MatchId) : IRequest<MatchGetDTO>;
    public class GetMatchByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetMatchById, MatchGetDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<MatchGetDTO> Handle(GetMatchById request, CancellationToken cancellationToken)
        {
            Match match = await _unitOfWork.MatchRepository.GetById(request.MatchId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.MatchId), request.MatchId));

            MatchGetDTO response = _mapper.Map<MatchGetDTO>(match);
            return response;
        }
    }
}
