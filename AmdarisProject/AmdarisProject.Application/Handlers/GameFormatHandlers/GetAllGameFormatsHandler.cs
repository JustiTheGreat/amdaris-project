using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Domain.Models;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.Application.Handlers.GameFormatHandlers
{
    public record GetAllGameFormats()
       : IRequest<IEnumerable<GameFormatResponseDTO>>;
    public class GetAllGameFormatsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetAllGameFormats, IEnumerable<GameFormatResponseDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<GameFormatResponseDTO>> Handle(GetAllGameFormats request, CancellationToken cancellationToken)
        {
            IEnumerable<GameFormat> gameFormats = await _unitOfWork.GameFormatRepository.GetAll();
            IEnumerable<GameFormatResponseDTO> response = _mapper.Map<List<GameFormatResponseDTO>>(gameFormats);
            return response;
        }
    }
}
