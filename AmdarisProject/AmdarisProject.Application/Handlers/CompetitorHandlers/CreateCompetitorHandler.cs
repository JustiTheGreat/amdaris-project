using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.CreateDTOs.CompetitorCreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record CreateCompetitor(CompetitorCreateDTO CompetitorCreateDTO) : IRequest<CompetitorGetDTO>;
    public class CreateCompetitorHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<CreateCompetitor, CompetitorGetDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<CompetitorGetDTO> Handle(CreateCompetitor request, CancellationToken cancellationToken)
        {
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

            CompetitorGetDTO response = created is Player ? _mapper.Map<PlayerGetDTO>(created)
                : created is Team ? _mapper.Map<TeamGetDTO>(created)
                : throw new AmdarisProjectException(nameof(created));

            return response;
        }
    }
}
