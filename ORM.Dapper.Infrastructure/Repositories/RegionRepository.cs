using Dapper;
using Microsoft.Extensions.Configuration;
using ORM.Dapper.Application.Interfaces;
using ORM.Dapper.Core.Models;
using System;
using System.Collections.Generic;
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

        public RegionRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<int> AddAsync(Region entity)
        {
            int affectedRows = 0;
            try
            {
                // TransactionScope guarantees commands can commit or roll back
                // as a single unit of work.
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    // "Open" enlists lightweight transaction, TransactionScope 
                    var sql = "INSERT INTO REGION (RegionDescription) VALUES (@RegionDescription)";
                    using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("NorthwindContext")))
                    {
                        connection.Open();
                        affectedRows = await connection.ExecuteAsync(sql, entity);
                    }

                    // Commits transaction; otherwise, rollback.
                    scope.Complete();
                }
            }
            catch (TransactionAbortedException ex)
            {
                Console.WriteLine("TransactionAbortedException Message: {0}", ex.Message);
            }

            return affectedRows;
        }

        public async Task<int> DeleteAsync(int id)
        {
            int result = 0;
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var sql = "DELETE FROM REGION WHERE RegionId = @Id";
                    using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("NorthwindContext")))
                    {
                        connection.Open();
                        result = await connection.ExecuteAsync(sql, new { Id = id });
                    }

                    // Commits transaction; otherwise, rollback.
                    scope.Complete();
                }
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
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var sql = "SELECT * FROM Region";
                    using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("NorthwindContext")))
                    {
                        connection.Open();
                        result = await connection.QueryAsync<Region>(sql);
                    }

                    scope.Complete();
                }
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
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var sql = "SELECT * FROM Region WHERE RegionId = @Id";
                    using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("NorthwindContext")))
                    {
                        connection.Open();
                        result = await connection.QuerySingleOrDefaultAsync<Region>(sql, new { Id = id });
                    }

                    scope.Complete();
                }
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
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var sql = "UPDATE Region SET RegionDescription = @Description WHERE Id = @Id";
                    using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("NorthwindContext")))
                    {
                        connection.Open();
                        affectedRows = await connection.ExecuteAsync(sql, entity);
                    }

                    scope.Complete();
                }
            }
            catch (TransactionAbortedException ex)
            {
                Console.WriteLine("TransactionAbortedException Message: {0}", ex.Message);
            }

            return affectedRows;
        }
    }
}
