using Grpc.Net.Client;
using gRPC_dotNet5;
using System;
using System.Threading.Tasks;

namespace gPRC_dotNet5_Client
{
    class Program
    {
       
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);
            var response = await client.SayHelloAsync(
                  new HelloRequest
                  {
                      Name = ".NET 5 - grpcClient"
                  });
            Console.WriteLine("From Server: " + response.Message);
            Console.ReadKey();
        }
    }
}
