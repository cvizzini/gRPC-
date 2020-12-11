using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Grpc.Core;
using Grpc.Net.Client;
using gRPC_dotNet5;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace gPRC_dotNet5_Client
{
    class Program
    {

        static async Task Main(string[] args)
        {
            var serverAddress = "https://localhost:5001";
            var channel = GrpcChannel.ForAddress(serverAddress);
            var client = new Greeter.GreeterClient(channel);
            var response = await client.SayHelloAsync(
                  new HelloRequest
                  {
                      Name = ".NET 5 - grpcClient"
                  });
            Console.WriteLine("From Server: " + response.Message);

            await CreditCardClient(serverAddress);
        }

        static async Task CreditCardClient(string serverAddress)
        {           
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                AppContext.SetSwitch(
                    "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
                serverAddress = "http://localhost:5000";
            }

            var channel = GrpcChannel.ForAddress(serverAddress);
            var client = new CreditRating.CreditRatingClient(channel);
            var creditRequest = new CreditRequest { Name = "id0201" };

            var accessToken = await GetAccessToken();
            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {accessToken}");

            var reply = await client.CheckCreditRequestAsync(creditRequest, headers);

            Console.WriteLine($"Credit for customer {creditRequest.Name} - {reply.Message}!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static IConfiguration GetAppSettings()
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json");

            return builder.Build();
        }

        static async Task<string> GetAccessToken()
        {
            var appAuth0Settings = GetAppSettings().GetSection("Auth0");
            var auth0Client = new AuthenticationApiClient(appAuth0Settings["Domain"]);
            var tokenRequest = new ClientCredentialsTokenRequest()
            {
                ClientId = appAuth0Settings["ClientId"],
                ClientSecret = appAuth0Settings["ClientSecret"],
                Audience = appAuth0Settings["Audience"]
            };
            var tokenResponse = await auth0Client.GetTokenAsync(tokenRequest);

            return tokenResponse.AccessToken;
        }
    }
}
