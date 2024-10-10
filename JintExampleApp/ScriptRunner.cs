using Microsoft.Extensions.Logging;

namespace JintExampleApp;

public class ScriptRunner(ILogger<ScriptRunner> logger) : IScriptRunner
{
    public void RunAllScripts(string folder)
    {
        logger.LogInformation("Running all scripts in folder {Folder}", folder);
    }
}

public interface IScriptRunner
{
    void RunAllScripts(string folder);
}