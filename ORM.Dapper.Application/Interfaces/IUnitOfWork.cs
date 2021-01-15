namespace ORM.Dapper.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IShipperRepository Shippers { get; }
        IRegionRepository Regions { get; }
        ITerritoryRepository Territories { get; }
    }
}
