namespace Exercise5.Models.DTOs
{
    public class RegisterNewProduct
    {
       
        public int idProduct {  get; set; }
        public int idWarehouse { get; set; }
        public int Amount { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
