using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;

namespace Geneva.Demo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            string loggingendpoint =
                Environment.GetEnvironmentVariable("FIRSTPARTY_LOGGING_GRPC_ENDPOINT")
                ?? "localhost";

            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddOpenTelemetry(
                    (opt) =>
                    {
                        opt.IncludeFormattedMessage = true;
                        opt.IncludeScopes = true;

                        AppContext.SetSwitch(
                            "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport",
                            true
                        );

                        var protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;

                        opt.AddOtlpExporter(otlpOptions =>
                        {
                            otlpOptions.Endpoint = new Uri($"http://{loggingendpoint}");
                            otlpOptions.Protocol = protocol;
                        });
                        opt.AddConsoleExporter();
                    }
                );
            });

            var logger = loggerFactory.CreateLogger<Program>();

            var counter = 0;
            var max = args.Length is not 0 ? Convert.ToInt32(args[0]) : -1;
            while (max is -1 || counter < max)
            {
                logger.LogInformation($"[{loggingendpoint}] OTEL Counter: {++counter}");
                await Task.Delay(TimeSpan.FromMilliseconds(1_000));
            }
        }
    }
}
