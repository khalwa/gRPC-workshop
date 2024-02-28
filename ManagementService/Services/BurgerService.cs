using BurgerGrpc;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ManagementService.Models;
using System.Timers;
using Timer = System.Timers.Timer;

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

        public override async Task SmartCooking(IAsyncStreamReader<SmartCookMessage> requestStream, IServerStreamWriter<SmartCookResponseMessage> responseStream, ServerCallContext context)
        {
            var counter = 0;

            _ = Task.Run(async () =>
            {
                while (true)
                {
                    if (counter >= 3)
                    {
                        await responseStream.WriteAsync(new SmartCookResponseMessage
                        {
                            Message = "Burger is burned now."
                        });
                        break;
                    } 
                    else if (counter == 2)
                    {
                        await responseStream.WriteAsync(new SmartCookResponseMessage
                        {
                            Message = "Burger needs flipping. NOW!"
                        });
                    }
                    else if (counter == 1)
                    {
                        await responseStream.WriteAsync(new SmartCookResponseMessage
                        {
                            Message = "Burger starts to fry."
                        });
                    }
                    
                    counter++;
                    await Task.Delay(1000);
                }
            });

            await foreach (var message in requestStream.ReadAllAsync())
            {
                if(message.Flip)
                {
                    counter = 0;
                    await responseStream.WriteAsync(new SmartCookResponseMessage
                    {
                        Message = "Burger was flipped."
                    });
                } else {
                    await responseStream.WriteAsync(new SmartCookResponseMessage
                    {
                        Message = "Burger was not flipped."
                    });
                }
            }
        }
    }
}
