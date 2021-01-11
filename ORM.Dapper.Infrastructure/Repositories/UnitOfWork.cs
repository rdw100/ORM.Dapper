using ORM.Dapper.Application.Interfaces;

namespace ORM.Dapper.Infrastructure.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        public UnitOfWork(IShipperRepository shipperRepository, IRegionRepository regionRepository)
        {
            Shippers = shipperRepository;
            Regions = regionRepository;
        }

        public IShipperRepository Shippers { get; }
        public IRegionRepository Regions { get; }
    }
}
