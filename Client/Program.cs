using Grpc.Net.Client;
using ManagementGrpc;
using Google.Protobuf.WellKnownTypes;
using BurgerGrpc;

Console.WriteLine("Burger Kniaź 1.0");

var channel = GrpcChannel.ForAddress("http://localhost:5000");

var helloClient = new Hello.HelloClient(channel);
Console.WriteLine(helloClient.SayHello(new Empty()));

var burgerClient = new Burger.BurgerClient(channel);
Console.WriteLine(burgerClient.PlaceOrder(new PlaceOrderMessage { Type = OrderType.Burger }));
