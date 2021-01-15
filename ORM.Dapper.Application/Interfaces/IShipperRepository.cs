using ORM.Dapper.Core.Models;
using System.Threading.Tasks;

namespace ORM.Dapper.Application.Interfaces
{
    public interface IShipperRepository : IGenericRepository<Shipper>
    {
        Task<int> UpdateAsync(Shipper entity);
        Task<int> DeleteAsync(int id);
    }
}
