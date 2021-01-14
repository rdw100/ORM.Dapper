using ORM.Dapper.Core.Models;
using System.Threading.Tasks;

namespace ORM.Dapper.Application.Interfaces
{
    public interface IRegionRepository : IGenericRepository<Region>
    {
        Task<Region> GetTerritoriesByRegion(int id);
    }
}
