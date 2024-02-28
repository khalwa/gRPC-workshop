using Grpc.Net.Client;
using ManagementGrpc;
using Google.Protobuf.WellKnownTypes;
using BurgerGrpc;
using Grpc.Core;

Console.WriteLine("Burger Kniaź 1.0");

var channel = GrpcChannel.ForAddress("http://localhost:5000");

var helloClient = new Hello.HelloClient(channel);
Console.WriteLine(helloClient.SayHello(new Empty()));

var burgerClient = new Burger.BurgerClient(channel);
Console.WriteLine(burgerClient.PlaceOrder(new PlaceOrderMessage { Type = OrderType.Burger }));

burgerClient.PlaceOrder(new PlaceOrderMessage { Type = OrderType.Burger });
burgerClient.PlaceOrderAsync(new PlaceOrderMessage { Type = OrderType.Pocket });
burgerClient.PlaceOrderAsync(new PlaceOrderMessage { Type = OrderType.Burger });

var readyOrdersCall = burgerClient.GetReadyOrders(new Empty());
await foreach (var response in readyOrdersCall.ResponseStream.ReadAllAsync())
{
    Console.WriteLine("Order is ready: " + response.OrderId);
}

var smartCookingCall = burgerClient.SmartCooking();
await foreach (var response in smartCookingCall.ResponseStream.ReadAllAsync())
{
    Console.WriteLine(response.Message);
    if(response.Message == "Burger needs flipping. NOW!")
    {
        if(Random.Shared.Next(100) > 50)
        {
            Console.WriteLine("Cook flipped the burger.");
            await smartCookingCall.RequestStream.WriteAsync(new SmartCookMessage
            {
                Flip = true
            });
        } else
        {
            Console.WriteLine("Cook was too lazy and did not flip the burger.");
            await smartCookingCall.RequestStream.WriteAsync(new SmartCookMessage
            {
                Flip = false
            });
        }
    }

    if(response.Message == "Burger is burned now.")
    {
        break;
    }
}
await smartCookingCall.RequestStream.CompleteAsync();