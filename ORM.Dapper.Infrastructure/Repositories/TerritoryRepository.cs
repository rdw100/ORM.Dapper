using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using ORM.Dapper.Application.Interfaces;
using ORM.Dapper.Core.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ORM.Dapper.Infrastructure.Repositories
{
    /// <summary>
    /// Encapsulates the logic required to access TERRITORY data sources.
    /// </summary>
    /// <seealso cref="ORM.Dapper.Application.Interfaces.ITerritoryRepository" />
    /// <remarks>
    /// This class uses Dapper Contrib to extend Dapper CRUD with helper 
    /// methods.  Reduces manual SQL Statements for simple tables.
    /// </remarks>
    public class TerritoryRepository : ITerritoryRepository
    {
        private readonly IConfiguration configuration;
        private readonly IDbConnection connectionString;

        public TerritoryRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.connectionString = new SqlConnection(configuration.GetConnectionString("NorthwindContext"));
        }

        public async Task<int> AddAsync(Territory entity)
        {
            return await connectionString.InsertAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await connectionString.DeleteAsync(new Territory { TerritoryID = id });
        }

        public async Task<IReadOnlyList<Territory>> GetAllAsync()
        {
            var result = await connectionString.GetAllAsync<Territory>();
            return result.ToList();
        }

        public async Task<Territory> GetByIdAsync(int id)
        {
            return await connectionString.GetAsync<Territory>(id);
        }

        public async Task<bool> UpdateAsync(Territory entity)
        {
            return await connectionString.UpdateAsync<Territory>(entity);
        }
    }
}
