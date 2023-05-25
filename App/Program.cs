﻿using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;

namespace Geneva.Demo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var counter = 0;
            var max = args.Length is not 0 ? Convert.ToInt32(args[0]) : -1;
            while (max is -1 || counter < max)
            {
                Console.WriteLine($"OTEL Counter: {++counter}");
                await Task.Delay(TimeSpan.FromMilliseconds(1_000));
            }
        }
    }
}