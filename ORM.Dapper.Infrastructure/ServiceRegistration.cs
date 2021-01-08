using Microsoft.Extensions.DependencyInjection;
using ORM.Dapper.Application.Interfaces;
using ORM.Dapper.Infrastructure.Repositories;

namespace ORM.Dapper.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IShipperRepository, ShipperRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
