namespace OnlineBookShop.Application.Common.Models
{
    public class PagedRequest
    {
        public required int PageIndex { get; set; }

        public required int PageSize { get; set; }

        public required string ColumnNameForSorting { get; set; }

        public required string SortDirection { get; set; }

        public required RequestFilters RequestFilters { get; set; }

        public PagedRequest() => RequestFilters = new RequestFilters();
    }
}
