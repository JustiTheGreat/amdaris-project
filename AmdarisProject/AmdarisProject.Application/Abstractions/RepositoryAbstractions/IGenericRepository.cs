﻿using AmdarisProject.Application.Common.Models;

namespace AmdarisProject.Application.Abstractions.RepositoryAbstractions
{
    public interface IGenericRepository<T>
    {
        Task<T> Create(T item);
        Task<T?> GetById(Guid id);
        Task<IEnumerable<T>> GetAll();
        Task<Tuple<IEnumerable<T>, int>> GetPaginatedData(PagedRequest pagedRequest);
        Task Delete(Guid id);
        Task<T> Update(T item);
    }
}