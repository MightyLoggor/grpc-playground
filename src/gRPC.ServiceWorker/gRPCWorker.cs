using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using gRPC.Model;
using gRPC.ServiceWorker.Services;

namespace gRPC.ServiceWorker
{
    public class gRPCWorker : BackgroundService
    {
        private readonly ILogger<gRPCWorker> _logger;

        public gRPCWorker(ILogger<gRPCWorker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting up Kestrel for localhost gRPC connections...");
            try
            {
                await Host.CreateDefaultBuilder()
                     .ConfigureWebHostDefaults(builder =>
                     {
                         builder
                             .ConfigureKestrel(options =>
                             {
                                 options.ListenLocalhost(30051, listenOptions =>
                                 {
                                     listenOptions.Protocols = HttpProtocols.Http2;

                                 });
                             })
                             .UseKestrel()
                             .UseStartup<GrpcServerStartup>();
                     })
                     .Build()
                     .RunAsync(stoppingToken);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
    }

    public class GrpcServerStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GreeterService>();
            });
        }
    }
}
