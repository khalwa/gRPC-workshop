using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ManagementGrpc;

namespace ManagementService.Services
{
    public class HelloService : Hello.HelloBase
    {
        public override Task<HelloResponse> SayHello(Empty request, ServerCallContext context)
        {
            return Task.FromResult(new HelloResponse
            {
                Response = "Hello World!"
            });
        }
    }
}
