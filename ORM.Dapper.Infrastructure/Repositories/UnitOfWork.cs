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

        //private IRegionRepository _regions;

        public IShipperRepository Shippers { get; }
       // public IRegionRepository Regions => _regions;
        public IRegionRepository Regions { get; }
        //public IRegionRepository Regions
        //{
        //    get { return _regions ?? (_regions = new RegionRepository()); }
        //}
    }
}
