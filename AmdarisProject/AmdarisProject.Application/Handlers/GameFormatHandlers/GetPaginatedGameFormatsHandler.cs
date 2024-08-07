﻿using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Common.Models;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Domain.Models;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.GameFormatHandlers
{
    public record GetPaginatedGameFormats(PagedRequest PagedRequest)
       : IRequest<PaginatedResult<GameFormatGetDTO>>;
    public class GetPaginatedGameFormatsHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPaginatedGameFormatsHandler> logger)
        : IRequestHandler<GetPaginatedGameFormats, PaginatedResult<GameFormatGetDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetPaginatedGameFormatsHandler> _logger = logger;

        public async Task<PaginatedResult<GameFormatGetDTO>> Handle(GetPaginatedGameFormats request, CancellationToken cancellationToken)
        {
            Tuple<IEnumerable<GameFormat>, int> gameFormats = await _unitOfWork.GameFormatRepository.GetPaginatedData(request.PagedRequest);
            IEnumerable<GameFormatGetDTO> mapped = _mapper.Map<IEnumerable<GameFormatGetDTO>>(gameFormats.Item1);
            PaginatedResult<GameFormatGetDTO> response = new()
            {
                Items = mapped,
                PageSize = request.PagedRequest.PageSize,
                PageIndex = request.PagedRequest.PageIndex,
                Total = gameFormats.Item2
            };

            _logger.LogInformation("Got paged game formats (Count = {Count})!", [response.Items.Count()]);

            return response;
        }
    }
}
