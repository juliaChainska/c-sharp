using Exercise5.Models;
using Exercise5.Models.DTOs;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Exercise5.Repository
{
    
    public interface IOrderRepository
    {
        Task Fulfill(int idOrder, DateTime createdAt);
        Task<Order> IsOrder(int productId, int amount, DateTime createdAt);
    }
    
    public class OrderRepository : IOrderRepository
    {
        private readonly IConfiguration _configuration;

        public OrderRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<Order>IsOrder(int productId, int amount, DateTime createdAt)
        {
            Order order = null;
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();


                command.CommandText = "SELECT * FROM [Order] where amount = " + amount + " and idProduct = " + productId + " and FulfilledAt IS NULL";
                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();

                await reader.ReadAsync();


                if (!reader.HasRows)
                {
                    return null;
                }

                int orderId = 0;


                if (reader.HasRows)
                {
                    orderId = int.Parse(reader["idOrder"].ToString());

                }
                await reader.CloseAsync();
                command.Parameters.Clear();

                order = new Models.Order
                {
                    idProduct = productId,
                    idOrder = orderId,
                    Amount = amount,
                    CreatedAt = createdAt,
                    FullfilledAt = DateTime.Now //?
                   
                };

                await reader.CloseAsync();
                command.Parameters.Clear();

            }
            return order; 
        }

        public async Task Fulfill(int idOrder, DateTime createdAt)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {

                var command = connection.CreateCommand();
                var transaction = (SqlTransaction)await connection.BeginTransactionAsync();
                command.Transaction = transaction;

                try
                {
                    command.CommandText = "UPDATE [Order] SET FulfilledAt = @CreatedAt WHERE IdOrder = @IdOrder";
                    command.Parameters.AddWithValue("CreatedAt", createdAt);
                    command.Parameters.AddWithValue("IdOrder", idOrder);

                    int rowsUpdated = await command.ExecuteNonQueryAsync();
                    command.Parameters.Clear();


                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Cos poszlo nie tak!");
                }
            }
        }

    }
}
