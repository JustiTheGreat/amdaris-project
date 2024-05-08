using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.DisplayDTOs;
using AmdarisProject.Domain.Models.CompetitionModels;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.CompetitionHandlers
{
    public record GetAllCompetitions() : IRequest<IEnumerable<CompetitionDisplayDTO>>;
    public class GetAllCompetitionsHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllCompetitionsHandler> logger)
        : IRequestHandler<GetAllCompetitions, IEnumerable<CompetitionDisplayDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetAllCompetitionsHandler> _logger = logger;

        public async Task<IEnumerable<CompetitionDisplayDTO>> Handle(GetAllCompetitions request, CancellationToken cancellationToken)
        {
            IEnumerable<Competition> competitions = await _unitOfWork.CompetitionRepository.GetAll();
            _logger.LogInformation("Got all competitions (Count = {Count})!", [competitions.Count()]);
            IEnumerable<CompetitionDisplayDTO> response = _mapper.Map<IEnumerable<CompetitionDisplayDTO>>(competitions);
            return response;
        }
    }
}
