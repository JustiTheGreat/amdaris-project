using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.GameFormatHandlers
{
    public record CreateGameFormat(GameFormatCreateDTO GameFormatCreateDTO)
        : IRequest<GameFormatGetDTO>;
    public class CreateGameFormatHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateGameFormatHandler> logger)
        : IRequestHandler<CreateGameFormat, GameFormatGetDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CreateGameFormatHandler> _logger = logger;

        public async Task<GameFormatGetDTO> Handle(CreateGameFormat request, CancellationToken cancellationToken)
        {
            bool validWinningConditions = request.GameFormatCreateDTO.WinAt is not null && request.GameFormatCreateDTO.WinAt > 0
                || request.GameFormatCreateDTO.DurationInMinutes is not null && request.GameFormatCreateDTO.DurationInMinutes > 0;

            if (!validWinningConditions)
                throw new APArgumentException(
                    [nameof(request.GameFormatCreateDTO.WinAt), nameof(request.GameFormatCreateDTO.DurationInMinutes)]);

            bool validCompetitorRequirements =
                request.GameFormatCreateDTO.CompetitorType is CompetitorType.PLAYER
                    && request.GameFormatCreateDTO.TeamSize is null
                || request.GameFormatCreateDTO.CompetitorType is CompetitorType.TEAM
                    && request.GameFormatCreateDTO.TeamSize is not null
                    && request.GameFormatCreateDTO.TeamSize >= 2;

            if (!validCompetitorRequirements)
                throw new APArgumentException(
                    [nameof(request.GameFormatCreateDTO.CompetitorType), nameof(request.GameFormatCreateDTO.TeamSize)]);

            GameFormat mapped = _mapper.Map<GameFormat>(request.GameFormatCreateDTO);
            GameFormat created;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                created = await _unitOfWork.GameFormatRepository.Create(mapped);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            _logger.LogInformation("Game format {GameFormatId} was created!", [created.Id]);

            GameFormatGetDTO response = _mapper.Map<GameFormatGetDTO>(created);
            return response;
        }
    }
}
