using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.DisplayDTOs;
using AmdarisProject.Domain.Models.CompetitionModels;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitionHandlers
{
    public record GetAllCompetitions() : IRequest<IEnumerable<CompetitionDisplayDTO>>;
    public class GetAllCompetitionsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetAllCompetitions, IEnumerable<CompetitionDisplayDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<CompetitionDisplayDTO>> Handle(GetAllCompetitions request, CancellationToken cancellationToken)
        {
            IEnumerable<Competition> competitions = await _unitOfWork.CompetitionRepository.GetAll();
            IEnumerable<CompetitionDisplayDTO> response = _mapper.Map<List<CompetitionDisplayDTO>>(competitions);
            return response;
        }
    }
}
