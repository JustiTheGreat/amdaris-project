using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.CreateDTOs.CompetitorCreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitorModels;
using Mapster;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record CreateCompetitor(CompetitorCreateDTO CompetitorCreateDTO) : IRequest<CompetitorResponseDTO>;
    public class CreateCompetitorHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<CreateCompetitor, CompetitorResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<CompetitorResponseDTO> Handle(CreateCompetitor request, CancellationToken cancellationToken)
        {
            Competitor mapped = request.CompetitorCreateDTO is PlayerCreateDTO ? request.CompetitorCreateDTO.Adapt<Player>()
                : request.CompetitorCreateDTO is TeamCreateDTO ? request.CompetitorCreateDTO.Adapt<Team>()
                : throw new AmdarisProjectException(nameof(CreateCompetitor), nameof(Handle), nameof(request.CompetitorCreateDTO));

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

            CompetitorResponseDTO response = created is Player ? created.Adapt<PlayerResponseDTO>()
                : created is Team ? created.Adapt<TeamResponseDTO>()
                : throw new AmdarisProjectException(nameof(CreateCompetitor), nameof(Handle), nameof(created));

            return response;
        }
    }
}
