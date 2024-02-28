using BurgerGrpc;

namespace ManagementService.Models
{
    public class Order
    {
        public int Id { get; set; }
        public OrderType Type { get; set; }
    }
}
