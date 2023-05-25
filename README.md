# Demo

<!-- TOC -->

- [Demo](#demo)
    - [Dotnet simple local OTLP exporter](#dotnet-simple-local-otlp-exporter)
    - [Simple containerized app that writes to the endpoint](#simple-containerized-app-that-writes-to-the-endpoint)

<!-- /TOC -->

## Dotnet simple local OTLP exporter

Let's run it inside Ubuntu.

```bash
cd "/mnt/d/Github/opentelemetry-dotnet/examples/Console"

docker run --rm -it -p 4317:4317 -p 4318:4318 -v $(pwd):/cfg otel/opentelemetry-collector:0.48.0 --config=/cfg/otlp-collector-example/config.yaml

# Unable to find image 'otel/opentelemetry-collector:0.48.0' locally
# 0.48.0: Pulling from otel/opentelemetry-collector
# 06c0b680202b: Pull complete
# e265c5593bec: Pull complete
# a24f534e3163: Pull complete
# Digest: sha256:6a5d51e901b419f1fdeec314436972c1f4056f1d137d9b200d0e43d33494ec3b
# Status: Downloaded newer image for otel/opentelemetry-collector:0.48.0
# 2023-05-25T20:05:26.280Z        info    builder/exporters_builder.go:255        Exporter was built.    {"kind": "exporter", "name": "logging"}
# 2023-05-25T20:05:26.280Z        info    builder/pipelines_builder.go:224        Pipeline was built.    {"kind": "pipeline", "name": "metrics"}
# 2023-05-25T20:05:26.280Z        info    builder/pipelines_builder.go:224        Pipeline was built.    {"kind": "pipeline", "name": "logs"}
# 2023-05-25T20:05:26.280Z        info    builder/pipelines_builder.go:224        Pipeline was built.    {"kind": "pipeline", "name": "traces"}
# 2023-05-25T20:05:26.280Z        info    builder/receivers_builder.go:226        Receiver was built.    {"kind": "receiver", "name": "otlp", "datatype": "metrics"}
# 2023-05-25T20:05:26.280Z        info    builder/receivers_builder.go:226        Receiver was built.    {"kind": "receiver", "name": "otlp", "datatype": "logs"}
# 2023-05-25T20:05:26.280Z        info    builder/receivers_builder.go:226        Receiver was built.    {"kind": "receiver", "name": "otlp", "datatype": "traces"}
# 2023-05-25T20:05:26.281Z        info    service/service.go:82   Starting extensions...
# 2023-05-25T20:05:26.281Z        info    service/service.go:87   Starting exporters...
# 2023-05-25T20:05:26.281Z        info    builder/exporters_builder.go:40 Exporter is starting... {"kind": "exporter", "name": "logging"}
# 2023-05-25T20:05:26.281Z        info    builder/exporters_builder.go:48 Exporter started.       {"kind": "exporter", "name": "logging"}
# 2023-05-25T20:05:26.281Z        info    service/service.go:92   Starting processors...
# 2023-05-25T20:05:26.281Z        info    builder/pipelines_builder.go:54 Pipeline is starting... {"kind": "pipeline", "name": "metrics"}
# 2023-05-25T20:05:26.281Z        info    builder/pipelines_builder.go:65 Pipeline is started.    {"kind": "pipeline", "name": "metrics"}
# 2023-05-25T20:05:26.281Z        info    builder/pipelines_builder.go:54 Pipeline is starting... {"kind": "pipeline", "name": "logs"}
# 2023-05-25T20:05:26.281Z        info    builder/pipelines_builder.go:65 Pipeline is started.    {"kind": "pipeline", "name": "logs"}
# 2023-05-25T20:05:26.281Z        info    builder/pipelines_builder.go:54 Pipeline is starting... {"kind": "pipeline", "name": "traces"}
# 2023-05-25T20:05:26.281Z        info    builder/pipelines_builder.go:65 Pipeline is started.    {"kind": "pipeline", "name": "traces"}
# 2023-05-25T20:05:26.281Z        info    service/service.go:97   Starting receivers...
# 2023-05-25T20:05:26.281Z        info    builder/receivers_builder.go:68 Receiver is starting... {"kind": "receiver", "name": "otlp"}
# 2023-05-25T20:05:26.281Z        info    otlpreceiver/otlp.go:69 Starting GRPC server on endpoint 0.0.0.0:4317   {"kind": "receiver", "name": "otlp"}
# 2023-05-25T20:05:26.281Z        info    otlpreceiver/otlp.go:87 Starting HTTP server on endpoint 0.0.0.0:4318   {"kind": "receiver", "name": "otlp"}
# 2023-05-25T20:05:26.281Z        info    builder/receivers_builder.go:73 Receiver started.       {"kind": "receiver", "name": "otlp"}
# 2023-05-25T20:05:26.281Z        info    service/telemetry.go:109        Setting up own telemetry...
# 2023-05-25T20:05:26.282Z        info    service/telemetry.go:129        Serving Prometheus metrics     {"address": ":8888", "level": "basic", "service.instance.id": "8600a599-6aa1-4f81-8cc8-0f82f2ac73c2", "service.version": "latest"}
# 2023-05-25T20:05:26.282Z        info    service/collector.go:252        Starting otelcol...     {"Version": "0.48.0", "NumCPU": 24}
# 2023-05-25T20:05:26.282Z        info    service/collector.go:142        Everything is ready. Begin running and processing data.

```

Now we run the code:

```powershell
cd "D:\Github\opentelemetry-dotnet\examples\Console"

dotnet run logs --useExporter otlp -e http://localhost:4317
```

On the other side, we see:

```bash
# ...
# 2023-05-25T20:05:26.282Z        info    service/collector.go:142        Everything is ready. Begin running and processing data.
# 2023-05-25T20:37:35.985Z        INFO    loggingexporter/logging_exporter.go:69  LogsExporter    {"#logs": 2}
# 2023-05-25T20:37:35.985Z        DEBUG   loggingexporter/logging_exporter.go:79  ResourceLog #0
# Resource SchemaURL:
# Resource labels:
#      -> telemetry.sdk.name: STRING(opentelemetry)
#      -> telemetry.sdk.language: STRING(dotnet)
#      -> telemetry.sdk.version: STRING(1.5.0-alpha.1.104)
#      -> service.name: STRING(unknown_service:Examples.Console)
# ScopeLogs #0
# ScopeLogs SchemaURL:
# InstrumentationScope
# LogRecord #0
# Timestamp: 2023-05-25 20:37:35.7626829 +0000 UTC
# Severity: Information
# ShortName:
# Body: Hello from tomato 2.99.
# Attributes:
#      -> dotnet.ilogger.category: STRING(Examples.Console.Program)
#      -> name: STRING(tomato)
#      -> price: DOUBLE(2.99)
#      -> {OriginalFormat}: STRING(Hello from {name} {price}.)
#      -> city: STRING(Seattle)
#      -> storeType: STRING(Physical)
# Trace ID:
# Span ID:
# Flags: 0
# LogRecord #1
# Timestamp: 2023-05-25 20:37:35.7662339 +0000 UTC
# Severity: Information
# ShortName:
# Body: Hello from potato 69.69.
# Attributes:
#      -> dotnet.ilogger.category: STRING(Examples.Console.Program)
#      -> name: STRING(potato)
#      -> price: DOUBLE(69.69)
#      -> {OriginalFormat}: STRING(Hello from {name} {price}.)
#      -> city: STRING(Seattle)
#      -> storeType: STRING(Physical)
# Trace ID:
# Span ID:
```

## Simple containerized app that writes to the endpoint

```powershell
cd "D:\Github\geneva-demo"

# Create a new console app
#
dotnet new console -o App -n Geneva.Demo

cd "App"

# Add packages
#
dotnet add package OpenTelemetry --version 1.5.0-alpha.2

# Run it
#
dotnet run

# Publish it - before building
#
dotnet publish -c Release

# Build the image
#
docker build -t geneva-demo -f Dockerfile .

# Tag and push
#
docker tag geneva-demo arcvspherelabacr.azurecr.io/arcdatalab/geneva-demo:latest
docker push arcvspherelabacr.azurecr.io/arcdatalab/geneva-demo:latest

# Run locally
#
docker run arcvspherelabacr.azurecr.io/arcdatalab/geneva-demo:latest
```