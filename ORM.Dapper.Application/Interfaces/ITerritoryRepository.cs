using ORM.Dapper.Core.Models;
using System.Threading.Tasks;

namespace ORM.Dapper.Application.Interfaces
{
    public interface ITerritoryRepository : IGenericRepository<Territory>
    {
        Task<bool> UpdateAsync(Territory entity);
        Task<bool> DeleteAsync(int id);
    }
}