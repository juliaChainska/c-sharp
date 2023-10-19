using Exercise5.Models;
using Exercise5.Models.DTOs;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Exercise5.Repository
{

    public interface IWarehouseRepository
    {
        Task<bool> Exists(int id);
        Task<Warehouse> IsWarehouse(int warehouseId);
    }
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly IConfiguration _configuration;
        public WarehouseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
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

        public async Task<Warehouse> IsWarehouse(int warehouseId)
        {

            
            Warehouse warehouse = null;

            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();

                
                command.CommandText = "select * from Warehouse where idWarehouse = " + warehouseId;
                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();

                await reader.ReadAsync();


                if (!reader.HasRows)
                {
                    return null;
                }

                string name = null;
                string address = null;
               

                if (reader.HasRows)
                {
                    name = reader["Name"].ToString();
                    address = (reader["Address"].ToString());
                    
                }
                await reader.CloseAsync();
                command.Parameters.Clear();

                warehouse = new Models.Warehouse
                {
                    idWarehouse = warehouseId,
                    Name = name,
                    Address = address

                };

                await reader.CloseAsync();
                command.Parameters.Clear();
               
            }
            return warehouse;
        }







    }
}
