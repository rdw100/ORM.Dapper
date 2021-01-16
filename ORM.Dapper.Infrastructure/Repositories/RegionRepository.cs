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
using System.Transactions;

namespace ORM.Dapper.Infrastructure.Repositories
{
    /// <summary>
    /// Encapsulates the logic required to access REGION data sources.
    /// </summary>
    /// <seealso cref="ORM.Dapper.Application.Interfaces.IRegionRepository" />
    /// <remarks>
    /// This class uses TransactionScope to make a code block transactional.
    /// </remarks>
    public class RegionRepository : IRegionRepository
    {
        private readonly IConfiguration configuration;
        private readonly IDbConnection connectionString;

        public RegionRepository(string connection)
        {
            this.connectionString = new SqlConnection(connection);
        }

        public RegionRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.connectionString = new SqlConnection(configuration.GetConnectionString("NorthwindContext"));
        }

        public async Task<int> AddAsync(Region entity)
        {
            int newId = 0;
            try
            {
                // TransactionScope guarantees commands can commit or roll back
                // as a single unit of work.
                using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                
                // "Open" enlists lightweight transaction, TransactionScope 
                var sql = "INSERT INTO REGION (RegionId, RegionDescription) VALUES (@RegionId, @RegionDescription);" +
                          "SELECT CAST(SCOPE_IDENTITY() as int)";

                using (var connection = connectionString)
                {
                    connection.Open();
                    newId = await connection.ExecuteAsync(sql, entity);
                    newId = entity.RegionID;
                }

                // Commits transaction; otherwise, rollback.
                scope.Complete();
            }
            catch (TransactionAbortedException ex)
            {
                Console.WriteLine("TransactionAbortedException Message: {0}", ex.Message);
            }

            return newId;
        }

        public async Task<int> DeleteAsync(int id)
        {
            int result = 0;
            try
            {
                using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                
                var sql = "DELETE FROM REGION WHERE RegionId = @Id";
                
                using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("NorthwindContext")))
                {
                    connection.Open();
                    result = await connection.ExecuteAsync(sql, new { Id = id });
                }

                // Commits transaction; otherwise, rollback.
                scope.Complete();
            }
            catch (TransactionAbortedException ex)
            {
                Console.WriteLine("TransactionAbortedException Message: {0}", ex.Message);
            }

            return result;
        }

        public async Task<IReadOnlyList<Region>> GetAllAsync()
        {
            IEnumerable<Region> result = null;
            try
            {
                using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                
                var sql = "SELECT * FROM Region";
                
                using (var connection = connectionString)
                {
                    connection.Open();
                    result = await connection.QueryAsync<Region>(sql);
                }

                scope.Complete();
            }
            catch (TransactionAbortedException ex)
            {
                Console.WriteLine("TransactionAbortedException Message: {0}", ex.Message);
            }

            return result.ToList();
        }

        public async Task<Region> GetByIdAsync(int id)
        {
            Region result = null;
            
            try
            {using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var sql = "SELECT * FROM Region WHERE RegionId = @Id";
                using (var connection = connectionString)
                {
                    connection.Open();
                    result = await connection.QuerySingleOrDefaultAsync<Region>(sql, new
                    {
                        Id = id
                    });
                }
                scope.Complete();
            }
            catch (TransactionAbortedException ex)
            {
                Console.WriteLine("TransactionAbortedException Message: {0}", ex.Message);
            }

            return result;
        }

        public async Task<int> UpdateAsync(Region entity)
        {
            int affectedRows = 0;
            try
            {
                using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                
                var sql = "UPDATE Region SET RegionDescription = @Description WHERE RegionId = @Id";
                
                using (var connection = connectionString)
                {
                    connection.Open();
                    affectedRows = await connection.ExecuteAsync(sql, entity);
                }

                scope.Complete();
            }
            catch (TransactionAbortedException ex)
            {
                Console.WriteLine("TransactionAbortedException Message: {0}", ex.Message);
            }

            return affectedRows;
        }

        public async void Save(Region region)
        {
            using var transaction = new TransactionScope();

            if (region.IsNew)
            {
                await this.AddAsync(region);
            }
            else
            {
                await this.UpdateAsync(region);
            }

            foreach (var terr in region.Territories.Where(a => !a.IsDeleted))
            {
                terr.RegionID = region.RegionID;

                if (terr.IsNew)
                {
                    this.Add(terr); // TODO: Make async.
                }
                else
                {
                    this.Update(terr); // TODO: Make async.
                }
            }

            foreach (var terr in region.Territories.Where(a => !a.IsDeleted))
            {
                this.connectionString.Execute("DELETE FROM Territory WHERE TerritoryId = @TerritoryId", new { terr.TerritoryID });
            }

            transaction.Complete();
        }

        public Territory Add(Territory territory)
        {
            var sql = "INSERT INTO Territory (TerritoryId, TerritoryDescription, RegionID)" +
                "VALUES (@TerritoryID, @TerritoryDescription, @RegionId); " +
                "SELECT CAST(SCOPE_IDENTITY() as int)";
            var id = this.connectionString.Query<int>(sql, territory).Single();
            territory.TerritoryID = id;
            return territory;
        }

        public Territory Update(Territory territory)
        {
            this.connectionString.Execute("UPDATE Territory " +
                "SET TerritoryDescription = @TerritoryDescription" +
                "    RegionId = @RegionID" +
                "WHERE TerritoryID = @TerritoryId");
            return territory;
        }

        public async Task<Region> GetTerritoriesByRegion(int id)
        {
            Region result = null;
            try
            {
                using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                
                var sql =
                    "SELECT * FROM Region WHERE RegionId = @Id; " +
                    "SELECT * FROM Territories WHERE RegionId = @Id";
                
                using (var connection = connectionString)
                {
                    connection.Open();
                    using (var multi = await connection.QueryMultipleAsync(sql, new { Id = id }))//.Result)
                    {
                        var regions = multi.Read<Region>().SingleOrDefault();
                        var territories = multi.Read<Territory>().ToList();
                        if (regions != null && territories != null)
                        {
                            regions.Territories.AddRange(territories);
                        }
                        result = regions;
                    }
                }

                scope.Complete();
            }
            catch (TransactionAbortedException ex)
            {
                Console.WriteLine("TransactionAbortedException Message: {0}", ex.Message);
            }

            return result;
        }
    }
}