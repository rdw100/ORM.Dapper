using ORM.Dapper.Application.Interfaces;

namespace ORM.Dapper.Infrastructure.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        public UnitOfWork(IShipperRepository shipperRepository)
        {
            Shippers = shipperRepository;
        }

        public IShipperRepository Shippers { get; }
    }
}
