using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AutoMapper;
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
            GameFormat mapped = _mapper.Map<GameFormat>(request.GameFormatCreateDTO);
            mapped.GameType = await _unitOfWork.GameTypeRepository.GetById(request.GameFormatCreateDTO.GameType)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.GameFormatCreateDTO.GameType), request.GameFormatCreateDTO.GameType));

            if (!mapped.HasValidWinningConditions())
                throw new APArgumentException(
                    [nameof(request.GameFormatCreateDTO.WinAt), nameof(request.GameFormatCreateDTO.DurationInMinutes)]);

            if (!mapped.HasValidCompetitorRequirements())
                throw new APArgumentException(
                    [nameof(request.GameFormatCreateDTO.CompetitorType), nameof(request.GameFormatCreateDTO.TeamSize)]);

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
