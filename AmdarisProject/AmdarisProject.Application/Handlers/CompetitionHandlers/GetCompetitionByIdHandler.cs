using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record GetCompetitionById(Guid CompetitionId) : IRequest<CompetitionResponseDTO>;
    public class GetCompetitionByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetCompetitionById, CompetitionResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<CompetitionResponseDTO> Handle(GetCompetitionById request, CancellationToken cancellationToken)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitionId), request.CompetitionId));

            CompetitionResponseDTO response = _mapper.Map<CompetitionResponseDTO>(competition);
            return response;
        }
    }
}
