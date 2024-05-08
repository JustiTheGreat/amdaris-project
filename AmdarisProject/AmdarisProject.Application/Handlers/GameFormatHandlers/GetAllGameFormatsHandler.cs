using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Domain.Models;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.GameFormatHandlers
{
    public record GetAllGameFormats()
       : IRequest<IEnumerable<GameFormatGetDTO>>;
    public class GetAllGameFormatsHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllGameFormatsHandler> logger)
        : IRequestHandler<GetAllGameFormats, IEnumerable<GameFormatGetDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetAllGameFormatsHandler> _logger = logger;

        public async Task<IEnumerable<GameFormatGetDTO>> Handle(GetAllGameFormats request, CancellationToken cancellationToken)
        {
            IEnumerable<GameFormat> gameFormats = await _unitOfWork.GameFormatRepository.GetAll();
            _logger.LogInformation("Got all game formats (Count = {Count})!", [gameFormats.Count()]);
            IEnumerable<GameFormatGetDTO> response = _mapper.Map<IEnumerable<GameFormatGetDTO>>(gameFormats);
            return response;
        }
    }
}
