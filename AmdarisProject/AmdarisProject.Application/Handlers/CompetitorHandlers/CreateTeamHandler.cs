using AmdarisProject.Application.Common.Abstractions;
using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record CreateTeam(CompetitorCreateDTO CompetitorCreateDTO) : IRequest<TeamGetDTO>;
    public class CreateTeamHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateTeamHandler> logger)
        : IRequestHandler<CreateTeam, TeamGetDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CreateTeamHandler> _logger = logger;

        public async Task<TeamGetDTO> Handle(CreateTeam request, CancellationToken cancellationToken)
        {
            Team mapped = _mapper.Map<Team>(request.CompetitorCreateDTO);

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

            _logger.LogInformation("Created team {TeamName}!", [created.Name]);

            TeamGetDTO response = _mapper.Map<TeamGetDTO>(created);
            return response;
        }
    }
}
