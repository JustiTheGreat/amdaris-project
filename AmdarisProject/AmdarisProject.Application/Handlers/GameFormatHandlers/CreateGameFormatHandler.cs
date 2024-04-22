using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.Application.Handlers.GameFormatHandlers
{
    public record CreateGameFormat(GameFormatCreateDTO GameFormatCreateDTO)
        : IRequest<GameFormatResponseDTO>;
    public class CreateGameFormatHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<CreateGameFormat, GameFormatResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<GameFormatResponseDTO> Handle(CreateGameFormat request, CancellationToken cancellationToken)
        {
            bool validWinningConditions = request.GameFormatCreateDTO.WinAt is not null
                || request.GameFormatCreateDTO.DurationInSeconds is not null;

            bool validCompetitorRequirements =
                request.GameFormatCreateDTO.CompetitorType is CompetitorType.PLAYER
                    && request.GameFormatCreateDTO.TeamSize is null
                || request.GameFormatCreateDTO.CompetitorType is CompetitorType.TEAM
                    && request.GameFormatCreateDTO.TeamSize is not null
                    && request.GameFormatCreateDTO.TeamSize >= 2;

            if (!validWinningConditions || !validCompetitorRequirements)
                throw new APArgumentException(nameof(request.GameFormatCreateDTO));

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

            GameFormatResponseDTO response = _mapper.Map<GameFormatResponseDTO>(created);
            return response;
        }
    }
}
