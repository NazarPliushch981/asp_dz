namespace asp_dz.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class OrderDto
    {
        public string Number { get; set; }
        public decimal Total { get; set; }
    }
}