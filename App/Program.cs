using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace LoggingOtelExporter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string tracingendpoint = Environment.GetEnvironmentVariable("FIRSTPARTY_TRACING_GRPC_ENDPOINT") ?? "localhost";
            string loggingendpoint = Environment.GetEnvironmentVariable("FIRSTPARTY_LOGGING_GRPC_ENDPOINT") ?? "localhost";

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.Services.AddOpenTelemetryTracing(
                        b =>
                        {
                            b.SetSampler(new AlwaysOnSampler())
                            .AddSource("Microsoft.AzureArcData.GenevaDemo.LoggingOtelExporter");
                        }
                    );

                builder.AddOpenTelemetry(options =>
                {
                    options.AddConsoleExporter();
                    options.AddOtlpExporter(
                            otelOptions => {
                                otelOptions.Endpoint = new Uri($"http://{loggingendpoint}");
                                otelOptions.Protocol = OtlpExportProtocol.Grpc;
                            }
                        );
                });
            });

            var serviceName = "Microsoft.AzureArcData.GenevaDemo";
            var serviceVersion = "1.0.0";


            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddSource(serviceName)
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
                .AddConsoleExporter()
                .AddOtlpExporter(config =>
                    {
                        config.Endpoint = new Uri($"http://{tracingendpoint}");
                        config.Protocol = OtlpExportProtocol.Grpc;
                    }
                )
                .Build();

            var MyActivitySource = new ActivitySource(serviceName);

            var logger = loggerFactory.CreateLogger("Log");

            logger.LogInformation($"FIRSTPARTY_TRACING_GRPC_ENDPOINT: {tracingendpoint} FIRSTPARTY_LOGGING_GRPC_ENDPOINT {loggingendpoint}");

            int counter = 0;
            while (true)
            {

                using var activity = MyActivitySource.StartActivity("SayHello");
                {
                    activity?.SetTag("Tag1", 1);
                    activity?.SetTag("Tag2", "Tagged message");

                    activity?.SetStatus(ActivityStatusCode.Ok);

                    logger.LogInformation($"[Logging endpoint: {loggingendpoint}] [Tracing endpoint: {tracingendpoint}] [OTEL: 1.3.0-rc.2] Counter: {++counter}");
                }

                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }
    }
}