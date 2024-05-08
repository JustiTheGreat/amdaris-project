using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record CreatePlayer(CompetitorCreateDTO CompetitorCreateDTO) : IRequest<PlayerGetDTO>;
    public class CreatePlayerHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreatePlayerHandler> logger)
        : IRequestHandler<CreatePlayer, PlayerGetDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CreatePlayerHandler> _logger = logger;

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

            _logger.LogInformation("Created player {PlayerName}!", [created.Name]);

            PlayerGetDTO response = _mapper.Map<PlayerGetDTO>(created);
            return response;
        }
    }
}
