using Exercise5.Models;
using Exercise5.Models.DTOs;
using Microsoft.AspNetCore.Hosting.Server;
using System.Data.SqlClient;

namespace Exercise5.Repository
{

    public interface IProductRepository
    {
        Task Create(Product product);
        //Product Get(Product product);
        Task<bool> Exists(int id);
        Task<Product> IsProduct(int productId);
    }

    public class ProductRepository : IProductRepository
    {

        private readonly IConfiguration _configuration;
        private readonly ProductRepository _productRepository;

        public ProductRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Create(Product product)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {

                var command = connection.CreateCommand();
                command.CommandText = "insert into product (name, description, price) values (@2,@3,@4)";
                command.Parameters.AddWithValue("@2", product.Name);
                command.Parameters.AddWithValue("@3", product.Description);
                command.Parameters.AddWithValue("@4", product.Price);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }


        }

        public async Task<bool> Exists(int id)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();
                command.CommandText = "select * from product where id = @1";
                command.Parameters.AddWithValue("@1", id);
                await connection.OpenAsync();
                if (await command.ExecuteScalarAsync() is not null)
                {
                    return true;
                }

                return false;
            }
        }

        public async Task<Product> IsProduct(int productId)
        {

            //var productId = newProduct.idProduct;
            Product prod = null;

            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();

                //product id
                command.CommandText = "select * from Product where idProduct = " + productId;
                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();

                await reader.ReadAsync();


                if (!reader.HasRows)
                {
                    return null;
                }

                string name = null;
                string description = null;
                decimal price = 0;

                if (reader.HasRows)
                {
                    name = reader["Name"].ToString();
                    description = (reader["Description"].ToString());
                    price = decimal.Parse(reader["Price"].ToString());
                }
                await reader.CloseAsync();
                command.Parameters.Clear();

                //if (!reader.HasRows) throw new NotFound("Produkt o podanym id nie istnieje");
                //await reader.CloseAsync();
                //command.Parameters.Clear();

                prod = new Models.Product
                {
                    idProduct = productId,
                    Name = name,
                    Description = description,
                    Price = price

                };

                await reader.CloseAsync();
                command.Parameters.Clear();
                //return prod;
            }
            return prod;
        }
    }
}
