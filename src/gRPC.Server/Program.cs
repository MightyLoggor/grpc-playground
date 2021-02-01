using System;
using System.Threading;
using System.Threading.Tasks;
using gRPC.Model;
using Grpc.Core;

namespace gRPC.Server
{
    class Program
    {
        const int port = 30051;

        static void Main(string[] args)
        {
            Grpc.Core.Server server = new Grpc.Core.Server
            {
                Services = { Greeter.BindService(new GreeterImpl()) },
                Ports = { new ServerPort("localhost", port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("Server is now listening on port: " + port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }

    class GreeterImpl : Greeter.GreeterBase
    {
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            Console.WriteLine($"[CLIENT] {request.Name}");

            return Task.FromResult(new HelloReply { Message = $"Olá {request.Name}, espero que tenho um ótimo dia!" });
        }
    }
}
