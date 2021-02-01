using System;
using Grpc.Core;
using gRPC.Model;

namespace gRPC.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Channel channel = new Channel("localhost:30051", ChannelCredentials.Insecure);

            var client = new Greeter.GreeterClient(channel);

            string nome = "";

            do
            {
                Console.WriteLine("\nDiga seu nome:");
                nome = Console.ReadLine();
                try
                {
                    var reply = client.SayHello(new HelloRequest { Name = nome });
                    Console.WriteLine($"[SERVER] {reply.Message}");
                }
                catch (Grpc.Core.RpcException e)
                {
                    Console.WriteLine($"[Error] STATUS_CODE: {e.Status.StatusCode} » {e.Status.Detail}");
                }
            } while (nome != "sair");

            channel.ShutdownAsync().Wait();
        }
    }
}
