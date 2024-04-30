using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Domain.Models;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.Application.Handlers.GameFormatHandlers
{
    public record GetAllGameFormats()
       : IRequest<IEnumerable<GameFormatGetDTO>>;
    public class GetAllGameFormatsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetAllGameFormats, IEnumerable<GameFormatGetDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<GameFormatGetDTO>> Handle(GetAllGameFormats request, CancellationToken cancellationToken)
        {
            IEnumerable<GameFormat> gameFormats = await _unitOfWork.GameFormatRepository.GetAll();
            IEnumerable<GameFormatGetDTO> response = _mapper.Map<List<GameFormatGetDTO>>(gameFormats);
            return response;
        }
    }
}
