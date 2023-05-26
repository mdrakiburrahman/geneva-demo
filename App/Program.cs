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

            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport",
                true
            );

            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddOpenTelemetry(
                    (opt) =>
                    {
                        opt.IncludeFormattedMessage = true;
                        opt.IncludeScopes = true;

                        opt.AddOtlpExporter(otlpOptions =>
                        {
                            otlpOptions.Endpoint = new Uri($"http://{loggingendpoint}");
                            otlpOptions.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
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
                logger.LogInformation($"[{loggingendpoint}] OTEL 1.3.0-rc.2 Counter: {++counter}");
                await Task.Delay(TimeSpan.FromMilliseconds(1_0000));
            }
        }
    }
}
