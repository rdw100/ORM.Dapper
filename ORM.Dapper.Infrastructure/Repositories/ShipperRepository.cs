using Dapper;
using Microsoft.Extensions.Configuration;
using ORM.Dapper.Application.Interfaces;
using ORM.Dapper.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ORM.Dapper.Infrastructure.Repositories
{
    /// <summary>
    /// Encapsulates the logic required to access SHIPPER data sources.
    /// </summary>
    /// <seealso cref="ORM.Dapper.Application.Interfaces.IShipperRepository" />
    /// <remarks>
    /// This class uses System Transactions to make a code block transactional.
    /// </remarks>
    public class ShipperRepository : IShipperRepository
    {
        private readonly IConfiguration configuration;
        private readonly IDbConnection connectionString;

        public ShipperRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.connectionString = new SqlConnection(configuration.GetConnectionString("NorthwindContext"));
        }

        public async Task<int> AddAsync(Shipper entity)
        {
            var sql = "SaveShipper";
            using (var connection = connectionString)
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var affectedRows = 0;
                    try
                    {
                        affectedRows = await connection.ExecuteAsync(sql, entity, transaction, commandType: CommandType.StoredProcedure);
                        transaction.Commit();

                        return affectedRows;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                    return affectedRows;
                }
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DeleteShipper";
            using (var connection = connectionString)
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, new { ShipperId = id }, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<IReadOnlyList<Shipper>> GetAllAsync()
        {
            var sql = "GetShippers";
            using (var connection = connectionString)
            {
                connection.Open();
                var result = await connection.QueryAsync<Shipper>(sql, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<Shipper> GetByIdAsync(int id)
        {
            var sql = "GetShipperByID";
            using (var connection = connectionString)
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<Shipper>(sql, new { ShipperId = id }, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<int> UpdateAsync(Shipper entity)
        {
            var sql = "SaveShipper";
            using (var connection = connectionString)
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, entity, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
    }
}
