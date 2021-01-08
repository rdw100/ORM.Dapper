namespace ORM.Dapper.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IShipperRepository Shippers { get; }
    }
}
