﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace ORM.Dapper.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<int> AddAsync(T entity);
    }
}
