using Exercise5.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Exercise5.Models;
using Exercise5.Models.DTOs;
using System.Transactions;
using System.Reflection.Metadata.Ecma335;

namespace Exercise5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IProductRepository _productRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductWarehouseRepository _productWarehouseRepository;


        public WarehousesController(IConfiguration configuration, IProductRepository productrepository, IWarehouseRepository warehouserepository, IOrderRepository orderrepository)
        {
            _configuration = configuration;
            _productRepository = productrepository;
            _warehouseRepository = warehouserepository;
            _orderRepository = orderrepository;
            
        }


        [HttpGet]
        public async Task<IActionResult> ShowProducts()
        {
            var products = new List<Product>();
            var warehouses = new List<Warehouse>();

            ////show products
            //using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            //{

            //    var command = connection.CreateCommand();
            //    command.CommandText = "select * from product";
            //    await connection.OpenAsync();
            //    var reader = await command.ExecuteReaderAsync();

            //    while (await reader.ReadAsync())
            //    {
            //        products.Add(new Product
            //        {
            //            idProduct = reader.GetInt32(0),
            //            Name = reader.GetString(1),
            //            Description = reader.GetString(2),
            //            Price = reader.GetDecimal(3)
            //        });
            //    }

            //}
            //return Ok(products);



            //show warehouse
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {

                var command = connection.CreateCommand();
                command.CommandText = "select * from warehouse";
                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    warehouses.Add(new Warehouse
                    {
                        idWarehouse = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Address = reader.GetString(2),
                        
                    });
                }

            }
            return Ok(warehouses);
        }


        [HttpPost]
        public async Task<IActionResult> RegisterProduct(RegisterNewProduct newProduct)
        {


            var product = await _productRepository.IsProduct(newProduct.idProduct);
            var warehouse = await _warehouseRepository.IsWarehouse(newProduct.idWarehouse);

            if (product == null)
            {
                return NotFound("Produkt o podanym id nie istnieje");
            }

            if (warehouse == null)
            {
                return NotFound("Warehouse o podanym id nie istnieje");
            }

            //amount
            var amount = newProduct.Amount;
            if (amount <= 0) return BadRequest("Amount nie moze byc mniejsze niz 0");

            //order
            var order = await _orderRepository.IsOrder(newProduct.idProduct, newProduct.Amount, newProduct.CreatedAt);
            if (order == null)
            {
                return NotFound("Order nie istnieje");
            }

            //using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            //{
            //    await _orderRepository.Fulfill(order.idOrder, order.CreatedAt);
            //    var newRecordId = await _productWarehouseRepository.Create2(new Models.ProductWarehouse
            //    {
            //        idWarehouse = warehouse.idWarehouse,
            //        idProduct = product.idProduct,
            //        idOrder = order.idOrder,
            //        Amount = newProduct.Amount,
            //        Price = newProduct.Amount * product.Price,
            //        CreatedAt = DateTime.Now
            //    });
            //    scope.Complete();
            //    return Created("", newRecordId);
            //}
            return Ok();
        }
    }
    }

