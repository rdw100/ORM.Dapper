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
        
        // Load Series of two or more tasks/processes
        //public async Task<int> LoadRegionTerritoryData() 
        //{
        //    int returnValue = await CreateTransactionScope(
        //        configuration.GetConnectionString("NorthwindContext"),
        //        configuration.GetConnectionString("NorthwindContext"),
        //        "UPDATE Region SET RegionDescription = 'Panhandle' WHERE RegionId = 99",
        //        "UPDATE Territories SET TerritoryDescription = '' WHERE TerritoryId = 99");
        //    return returnValue;
        //}

        // This function takes arguments for 2 connection strings and commands to create a transaction 
        // involving two SQL Servers. It returns a value > 0 if the transaction is committed, 0 if the 
        // transaction is rolled back. To test this code, you can connect to two different databases 
        // on the same server by altering the connection string, or to another 3rd party RDBMS by 
        // altering the code in the connection2 code block.
        static public int CreateTransactionScope(
            string connectString1, string connectString2,
            string commandText1, string commandText2)
        {
            // Initialize the return value to zero and create a StringWriter to display results.
            int returnValue = 0;
            System.IO.StringWriter writer = new System.IO.StringWriter();

            try
            {
                // Create the TransactionScope to execute the commands, guaranteeing
                // that both commands can commit or roll back as a single unit of work.
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection connection1 = new SqlConnection(connectString1))
                    {
                        // Opening the connection automatically enlists it in the 
                        // TransactionScope as a lightweight transaction.
                        connection1.Open();

                        // Create the SqlCommand object and execute the first command.
                        SqlCommand command1 = new SqlCommand(commandText1, connection1);
                        returnValue = command1.ExecuteNonQuery();
                        writer.WriteLine("Rows to be affected by command1: {0}", returnValue);

                        // If you get here, this means that command1 succeeded. By nesting
                        // the using block for connection2 inside that of connection1, you
                        // conserve server and network resources as connection2 is opened
                        // only when there is a chance that the transaction can commit.   
                        using (SqlConnection connection2 = new SqlConnection(connectString2))
                        {
                            // The transaction is escalated to a full distributed
                            // transaction when connection2 is opened.
                            connection2.Open();

                            // Execute the second command in the second database.
                            returnValue = 0;
                            SqlCommand command2 = new SqlCommand(commandText2, connection2);
                            returnValue = command2.ExecuteNonQuery();
                            writer.WriteLine("Rows to be affected by command2: {0}", returnValue);
                        }
                    }

                    // The Complete method commits the transaction. If an exception has been thrown,
                    // Complete is not  called and the transaction is rolled back.
                    scope.Complete();
                }
            }
            catch (TransactionAbortedException ex)
            {
                writer.WriteLine("TransactionAbortedException Message: {0}", ex.Message);
            }

            // Display messages.
            Console.WriteLine(writer.ToString());

            return returnValue;
        }
    }
}
