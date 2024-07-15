using AmdarisProject.Application.Common.Models;
using AmdarisProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Text;

namespace AmdarisProject.Infrastructure.Persistance.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<Tuple<IEnumerable<T>, int>> CreatePaginatedResultAsync<T>(this IQueryable<T> query, PagedRequest pagedRequest)
            where T : Model
        {
            IQueryable<T> totalItems = query.ApplyFilters(pagedRequest).Sort(pagedRequest);
            int total = await totalItems.CountAsync();
            IEnumerable<T> paginatedItems = await totalItems.Paginate(pagedRequest).ToListAsync();
            return Tuple.Create(paginatedItems, total);
        }


        private static IQueryable<T> Paginate<T>(this IQueryable<T> query, PagedRequest pagedRequest)
            => query.Skip(pagedRequest.PageIndex * pagedRequest.PageSize).Take(pagedRequest.PageSize);

        private static IQueryable<T> Sort<T>(this IQueryable<T> query, PagedRequest pagedRequest)
            => string.IsNullOrEmpty(pagedRequest.ColumnNameForSorting) ? query
                : query.OrderBy(pagedRequest.ColumnNameForSorting + " " + pagedRequest.SortDirection);

        private static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, PagedRequest pagedRequest)
        {
            var requestFilters = pagedRequest.RequestFilters;
            var predicate = new StringBuilder();

            for (int i = 0; i < requestFilters.Filters.Count; i++)
            {
                if (i > 0)
                    predicate.Append($" {requestFilters.LogicalOperator} ");

                predicate.Append($"{requestFilters.Filters[i].Path}.{nameof(string.Contains)}(@{i})");
            }

            if (requestFilters.Filters.Any())
            {
                var propertyValues = requestFilters.Filters.Select(filter => filter.Value).ToArray();
                query = query.Where(predicate.ToString(), propertyValues);
            }

            return query;
        }
    }
}
