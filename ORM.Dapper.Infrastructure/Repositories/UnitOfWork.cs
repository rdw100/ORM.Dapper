using ORM.Dapper.Application.Interfaces;

namespace ORM.Dapper.Infrastructure.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        public UnitOfWork(IShipperRepository shipperRepository, 
            IRegionRepository regionRepository,
            ITerritoryRepository territoryRepository)
        {
            Shippers = shipperRepository;
            Regions = regionRepository;
            Territories = territoryRepository;
        }

        public IShipperRepository Shippers { get; }
        public IRegionRepository Regions { get; }
        public ITerritoryRepository Territories { get; }
    }
}
