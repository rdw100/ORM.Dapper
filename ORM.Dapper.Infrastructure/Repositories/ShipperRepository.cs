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
            var parameters = new DynamicParameters();
            parameters.Add("@ShipperID", 
                            value: entity.ShipperID, 
                            dbType: DbType.Int32, 
                            direction: ParameterDirection.Output);
            parameters.Add("@CompanyName", entity.CompanyName);
            parameters.Add("@Phone", entity.Phone);
            entity.ShipperID = parameters.Get<int>("@ShipperID");

            var sql = "InsertShipper";
            using var connection = connectionString;
            connection.Open();
            using var transaction = connection.BeginTransaction();
            var affectedRows = 0;
            try
            {
                affectedRows = await connection.ExecuteAsync(
                    sql,
                    parameters,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
            return affectedRows;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DeleteShipper";
            using var connection = connectionString;
            connection.Open();
            using var transaction = connection.BeginTransaction();
            var result = 0;
            try
            {
                result = await connection.ExecuteAsync(sql,
                    new { ShipperId = id },
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
            return result;
        }

        public async Task<IReadOnlyList<Shipper>> GetAllAsync()
        {
            var sql = "GetShippers";
            using var connection = connectionString;
            connection.Open();
            using var transaction = connection.BeginTransaction();
            IEnumerable<Shipper> result = null;
            try
            {
                result = await connection.QueryAsync<Shipper>(
                    sql,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
            return result.ToList();
        }

        public async Task<Shipper> GetByIdAsync(int id)
        {
            var sql = "GetShipperByID";
            using var connection = connectionString;
            connection.Open();
            using var transaction = connection.BeginTransaction();
            Shipper result = null;
            try
            {
                result = await connection.QuerySingleOrDefaultAsync<Shipper>(
                    sql,
                    new { ShipperId = id },
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
            return result;
        }

        public async Task<int> UpdateAsync(Shipper entity)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ShipperID",
                            value: entity.ShipperID,
                            dbType: DbType.Int32,
                            direction: ParameterDirection.Input);
            parameters.Add("@CompanyName", entity.CompanyName);
            parameters.Add("@Phone", entity.Phone);
            entity.ShipperID = parameters.Get<int>("@ShipperID");

            var sql = "UpdateShipper";
            using var connection = connectionString;
            connection.Open();
            using var transaction = connection.BeginTransaction();
            var result = 0;
            try
            {
                result = await connection.ExecuteAsync(
                    sql,
                    parameters,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
            return result;
        }
    }
}