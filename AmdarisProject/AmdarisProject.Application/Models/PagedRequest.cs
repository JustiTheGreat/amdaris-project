using AmdarisProject.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AmdarisProject.Application.Common.Models
{
    public class PagedRequest
    {
        [Required]
        public required int PageIndex { get; set; }

        [Required]
        public required int PageSize { get; set; }

        public required string ColumnNameForSorting { get; set; }

        public required SortDirection SortDirection { get; set; }

        public RequestFilters RequestFilters { get; set; }

        public PagedRequest() => RequestFilters = new RequestFilters();
    }
}
