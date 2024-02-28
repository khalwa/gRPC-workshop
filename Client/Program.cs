using Grpc.Net.Client;
using ManagementGrpc;
using Google.Protobuf.WellKnownTypes;

Console.WriteLine("Hello, World!");

var channel = GrpcChannel.ForAddress("http://localhost:5000");
var client = new Hello.HelloClient(channel);

Console.WriteLine(client.SayHello(new Empty()));