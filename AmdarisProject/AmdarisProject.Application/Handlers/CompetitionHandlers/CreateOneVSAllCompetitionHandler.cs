using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.CompetitionHandlers
{
    public record CreateOneVSAllCompetition(CompetitionCreateDTO CompetitionCreateDTO)
        : IRequest<OneVSAllCompetitionGetDTO>;
    public class CreateOneVSAllCompetitionHandler(IUnitOfWork unitOfWork, IMapper mapper,
        ILogger<CreateOneVSAllCompetitionHandler> logger)
        : IRequestHandler<CreateOneVSAllCompetition, OneVSAllCompetitionGetDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CreateOneVSAllCompetitionHandler> _logger = logger;

        public async Task<OneVSAllCompetitionGetDTO> Handle(CreateOneVSAllCompetition request, CancellationToken cancellationToken)
        {
            OneVSAllCompetition mapped = _mapper.Map<OneVSAllCompetition>(request.CompetitionCreateDTO);
            mapped.GameFormat = await _unitOfWork.GameFormatRepository.GetById(request.CompetitionCreateDTO.GameFormat)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitionCreateDTO.GameFormat), request.CompetitionCreateDTO.GameFormat));

            if (mapped.BreakInMinutes is null ^ mapped.GameFormat.DurationInMinutes is null)
                throw new APArgumentException([nameof(mapped.BreakInMinutes), nameof(mapped.GameFormat.DurationInMinutes)]);

            Competition competition;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                competition = await _unitOfWork.CompetitionRepository.Create(mapped);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            _logger.LogInformation("Created one vs all competition {CompetitionName}!", [competition.Name]);

            OneVSAllCompetitionGetDTO response = _mapper.Map<OneVSAllCompetitionGetDTO>(competition);
            return response;
        }
    }
}
