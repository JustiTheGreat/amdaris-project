using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Utils;
using AmdarisProject.Domain.Models.CompetitorModels;
using Mapster;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record GetCompetitionFirstPlaceCompetitors(ulong CompetitionId) : IRequest<IEnumerable<CompetitorResponseDTO>>;
    public class GetCompetitionFirstPlaceCompetitorsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetCompetitionFirstPlaceCompetitors, IEnumerable<CompetitorResponseDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<CompetitorResponseDTO>> Handle(GetCompetitionFirstPlaceCompetitors request, CancellationToken cancellationToken)
        {
            IEnumerable<Competitor> firstPlaceCompetitors =
                await HandlerUtils.GetCompetitionFirstPlaceCompetitorsUtil(_unitOfWork, request.CompetitionId);
            IEnumerable<CompetitorResponseDTO> response = firstPlaceCompetitors.Select(_mapper.Map<CompetitorResponseDTO>).ToList();
            return response;
        }
    }
}
