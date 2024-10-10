using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddLogging(logBuilder =>
{
    var logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().CreateLogger();
    logBuilder.ClearProviders().AddSerilog(logger);
});

var host = builder.Build();

var cts = new CancellationTokenSource();
var scriptFolder = @"C:\Dev\Codetheme\JintExample\JintExampleApp\Scripts)";
while (!cts.IsCancellationRequested)
{
    var input = Console.ReadLine();
    if (input == "e")
    {
        cts.Cancel();
    }
}