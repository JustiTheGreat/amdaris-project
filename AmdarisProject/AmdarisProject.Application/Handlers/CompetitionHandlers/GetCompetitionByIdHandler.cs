using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record GetCompetitionById(Guid CompetitionId) : IRequest<CompetitionGetDTO>;
    public class GetCompetitionByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetCompetitionById, CompetitionGetDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<CompetitionGetDTO> Handle(GetCompetitionById request, CancellationToken cancellationToken)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitionId), request.CompetitionId));

            CompetitionGetDTO response = competition is OneVSAllCompetition ? _mapper.Map<OneVSAllCompetitionResponseDTO>(competition)
                : competition is TournamentCompetition ? _mapper.Map<TournamentCompetitionResponseDTO>(competition)
                : throw new AmdarisProjectException("Unexpected competition type!");
            return response;
        }
    }
}
