using Exercise5.Models;
using Exercise5.Models.DTOs;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Exercise5.Repository
{
    public interface IProductWarehouseRepository
    {
        
        public Task Create2(ProductWarehouse newProduct);

    }
    public class ProductWarehouseRepository : IProductWarehouseRepository
    {
        private readonly IConfiguration _configuration;
        public ProductWarehouseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }


       
        public async Task Create2(ProductWarehouse newProduct)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {

                var command = connection.CreateCommand();
                var transaction = (SqlTransaction)await connection.BeginTransactionAsync();
                command.Transaction = transaction;

                try
                {

                    command.CommandText = "INSERT INTO Product_Warehouse(IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) " +
                    $"VALUES(@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt)";

                    command.Parameters.AddWithValue("IdWarehouse", newProduct.idWarehouse);
                    command.Parameters.AddWithValue("IdProduct", newProduct.idProduct);
                    command.Parameters.AddWithValue("IdOrder", newProduct.idOrder);
                    command.Parameters.AddWithValue("Price", newProduct.Price);
                    command.Parameters.AddWithValue("Amount", newProduct.Amount);
                    command.Parameters.AddWithValue("CreatedAt", newProduct.CreatedAt);

         

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception("Cos poszlo nie tak");
                }
            }

        }
    }
}
