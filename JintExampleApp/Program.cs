using JintExampleApp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddTransient<IScriptRunner, ScriptRunner>();

builder.Services.AddLogging(logBuilder =>
{
    var logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console(theme: AnsiConsoleTheme.Sixteen)
        .CreateLogger();
    
    logBuilder.ClearProviders().AddSerilog(logger);
});

var host = builder.Build();

var cts = new CancellationTokenSource();
var scriptFolder = @"C:\Dev\Codetheme\JintExample\JintExampleApp\Scripts";
var scriptRunner = host.Services.GetRequiredService<IScriptRunner>();

while (!cts.IsCancellationRequested)
{
    var input = Console.ReadLine();
    if (input == "e")
    {
        cts.Cancel();
    }
    else if (input == "run")
    {
        scriptRunner.RunAllScripts(scriptFolder);
    }
}