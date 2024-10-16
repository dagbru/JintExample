using System.Text.Json;
using Jint;
using JintExampleApp.Model;
using Microsoft.Extensions.Logging;

namespace JintExampleApp;

public class ScriptRunner : IScriptRunner
{
    private readonly ILogger<ScriptRunner> _logger;
    private Dictionary<string, object> _scriptProperties = new();
    private List<ScriptWithReturnValues> _scriptWithReturnValues = new();

    public ScriptRunner(ILogger<ScriptRunner> logger)
    {
        _logger = logger;
        _scriptProperties.Add("person", new Person
        {
            FirstName = "Dag",
            MiddleName = "Brudal",
            LastName = "Sørensen",
            BirthYear = 1990
        });
        
        _scriptWithReturnValues.Add(new ScriptWithReturnValues
        {
            ScriptName = "generatePersons",
            ReturnType = typeof(IEnumerable<Person>)
        });
    }

    public Person GetDefaultPerson()
    {
        return new Person
        {
            FirstName = "Nick",
            MiddleName = "The Cage",
            LastName = "Cage",
            BirthYear = 1969
        };
    }

    public void RunAllScripts(string folder)
    {
        _logger.LogInformation("Running all scripts in folder {Folder}", folder);
        var di = new DirectoryInfo(folder);
        var scripts = di.EnumerateFiles();
        foreach (var script in scripts)
        {
            var scriptContent = File.ReadAllText(script.FullName);
            var scriptName = Path.GetFileNameWithoutExtension(script.Name);
            
            try
            {
                using var engine = InitializeEngine();
                var result = engine.Execute(scriptContent).Invoke(scriptName);

                var matchingScriptWithReturnValue =
                    _scriptWithReturnValues.FirstOrDefault(x => x.ScriptName == scriptName);
                if (matchingScriptWithReturnValue == null) continue;

                var deserializedScriptResult =
                    JsonSerializer.Deserialize(result.ToString(), matchingScriptWithReturnValue.ReturnType);
                _logger.LogDebug("Script {ScriptName} return {@ReturnValue}", scriptName, deserializedScriptResult);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Script {ScriptName} failed", scriptName);
            }
        }
        
        _logger.LogInformation("{@ScriptProperties}", _scriptProperties);
    }

    private Engine InitializeEngine()
    {
        Engine? engine = null;
        try
        {
            engine = new Engine();
            engine.SetValue("log", new Action<object>(a => _logger.LogDebug("{@LogText}", a)));
            engine.SetValue("getDefaultPerson", GetDefaultPerson);
            
            foreach (var scriptProperty in _scriptProperties)
            {
                engine.SetValue(scriptProperty.Key, scriptProperty.Value);
            }
            
            return engine;
        }
        catch
        {
            engine?.Dispose();
            throw;
        }
    }
}

public interface IScriptRunner
{
    void RunAllScripts(string folder);
}