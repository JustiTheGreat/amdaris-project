using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record CreatePlayer(CompetitorCreateDTO CompetitorCreateDTO) : IRequest<PlayerGetDTO>;
    public class CreatePlayerHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<CreatePlayer, PlayerGetDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<PlayerGetDTO> Handle(CreatePlayer request, CancellationToken cancellationToken)
        {
            Player mapped = _mapper.Map<Player>(request.CompetitorCreateDTO);

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

            PlayerGetDTO response = _mapper.Map<PlayerGetDTO>(created);
            return response;
        }
    }
}
