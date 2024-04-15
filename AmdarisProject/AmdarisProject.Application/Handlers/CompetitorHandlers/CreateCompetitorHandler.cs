using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.CreateDTOs.CompetitorCreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record CreateCompetitor(CompetitorCreateDTO CompetitorCreateDTO) : IRequest<CompetitorResponseDTO>;
    public class CreateCompetitorHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<CreateCompetitor, CompetitorResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<CompetitorResponseDTO> Handle(CreateCompetitor request, CancellationToken cancellationToken)
        {
            if (request.CompetitorCreateDTO is TeamCreateDTO teamCreateDTO && teamCreateDTO.TeamSize < 2)
                throw new APArgumentException(nameof(teamCreateDTO.TeamSize));

            Competitor mapped = request.CompetitorCreateDTO is PlayerCreateDTO ? _mapper.Map<Player>(request.CompetitorCreateDTO)
                : request.CompetitorCreateDTO is TeamCreateDTO ? _mapper.Map<Team>(request.CompetitorCreateDTO)
                : throw new APArgumentException(nameof(request.CompetitorCreateDTO));

            Competitor created;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                created = await _unitOfWork.CompetitorRepository.Create(mapped);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            CompetitorResponseDTO response = created is Player ? _mapper.Map<PlayerResponseDTO>(created)
                : created is Team ? _mapper.Map<TeamResponseDTO>(created)
                : throw new AmdarisProjectException(nameof(created));

            return response;
        }
    }
}
