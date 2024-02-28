using BurgerGrpc;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ManagementService.Models;

namespace ManagementService.Services
{
    public class BurgerService : Burger.BurgerBase
    {
        private static int OrderCounter = 1;
        private static Queue<Order> _queue = new Queue<Order>();

        public override Task<OrderPlacedMessage> PlaceOrder(PlaceOrderMessage request, ServerCallContext context)
        {
            var order = new Order
            {
                Id = OrderCounter++,
                Type = request.Type
            };

            _queue.Enqueue(order);

            var message = request.Type == OrderType.Burger ? "Smacznego burgera" : "Smacznego pocketa";
            return Task.FromResult(new OrderPlacedMessage
            {
                OrderId = 1,
                Cost = 25.23f,
                Date = Timestamp.FromDateTime(DateTime.UtcNow),
                Message = message
            });
        }

        public override async Task GetReadyOrders(Empty request, IServerStreamWriter<OrderReadyMessage> responseStream, ServerCallContext context)
        {
            while(_queue.Any()) {
                var order = _queue.Dequeue();
                await responseStream.WriteAsync(new OrderReadyMessage
                {
                    OrderId = order.Id
                });
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }
    }
}
