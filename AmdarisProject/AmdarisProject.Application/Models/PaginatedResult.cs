using AmdarisProject.Application.Dtos.ResponseDTOs;

namespace AmdarisProject.Application.Common.Models
{
    public class PaginatedResult<T> where T : ResponseDTO
    {
        public required int PageIndex { get; set; }
        public required int PageSize { get; set; }
        public required int Total { get; set; }
        public required IEnumerable<T> Items { get; set; }
    }
}
