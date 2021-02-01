using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gRPC.Model;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace gRPC.ServiceWorker.Services
{
    class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"[CLIENT] {request.Name}");

            return Task.FromResult(new HelloReply { Message = $"Olá {request.Name}, espero que tenho um ótimo dia!" });
        }
    }
}
