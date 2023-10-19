namespace Exercise5.Models
{
    public class ProductWarehouse
    {
        public int idProductWarehouse { get; set; }
        public int idWarehouse { get; set; }
        public int idProduct { get; set; }

        public int idOrder {get; set; }
        public int Amount { get; set; } 
        public decimal Price { get; set; } //amount*price
        public DateTime CreatedAt { get; set; } 


    }
}
