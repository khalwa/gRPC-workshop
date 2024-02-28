using BurgerGrpc;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace ManagementService.Services
{
    public class BurgerService : Burger.BurgerBase
    {
        public override Task<OrderPlacedMessage> PlaceOrder(PlaceOrderMessage request, ServerCallContext context)
        {
            var message = request.Type == OrderType.Burger ? "Smacznego burgera" : "Smacznego pocketa";
            return Task.FromResult(new OrderPlacedMessage
            {
                OrderId = 1,
                Cost = 25.23f,
                Date = Timestamp.FromDateTime(DateTime.UtcNow),
                Message = message
            });
        }
    }
}
