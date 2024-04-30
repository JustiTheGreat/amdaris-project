using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Services;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.Application.Handlers.MatchHandlers
{
    public record EndMatch(Guid MatchId, MatchStatus Status) : IRequest<MatchGetDTO>;
    public class EndMatchHandler(IUnitOfWork unitOfWork, IMapper mapper, IEndMatchService endMatchService)
        : IRequestHandler<EndMatch, MatchGetDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IEndMatchService _endMatchService = endMatchService;

        public async Task<MatchGetDTO> Handle(EndMatch request, CancellationToken cancellationToken)
        {
            Match updated;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                updated = await _endMatchService.End(request.MatchId, request.Status);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            MatchGetDTO response = _mapper.Map<MatchGetDTO>(updated);
            return response;
        }
    }
}
