namespace Exercise5.Models
{
    public class Order
    {
        public int idOrder { get; set; }
        public int idProduct { get; set; }
        public int Amount { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime FullfilledAt { get; set; }
      
    }
}
