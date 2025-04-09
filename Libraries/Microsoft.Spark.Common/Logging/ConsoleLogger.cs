using System.Reflection;
using System.Text.RegularExpressions;

namespace Microsoft.Spark.Common.Logging;

public partial class ConsoleLogger<T>(LogLevel level = LogLevel.Info) : ConsoleLogger(typeof(T).Name, level), ILogger<T>;
public partial class ConsoleLogger : ILogger
{
    public string Name { get; }
    public LogLevel Level { get; set; }

    protected Regex _pattern;

    public ConsoleLogger(string? name = null, LogLevel level = LogLevel.Info)
    {
        Name = name ?? Assembly.GetEntryAssembly()?.GetName().Name ?? "Microsoft.Spark";
        Level = Environment.GetEnvironmentVariable("LOG_LEVEL")?.ToLogLevel() ?? level;
        _pattern = ParseMagicExpression(Environment.GetEnvironmentVariable("LOG") ?? "*");
    }

    public ConsoleLogger(LoggingSettings settings)
    {
        Name = Assembly.GetEntryAssembly()?.GetName().Name ?? "Microsoft.Spark";
        Level = settings.Level;
        _pattern = ParseMagicExpression(settings.Enable);
    }

    public ConsoleLogger(IServiceProvider provider)
    {
        var settings = (LoggingSettings?)provider.GetService(typeof(LoggingSettings)) ?? new();
        Name = Assembly.GetEntryAssembly()?.GetName().Name ?? "Microsoft.Spark";
        Level = settings.Level;
        _pattern = ParseMagicExpression(settings.Enable);
    }

    public void Debug(params object?[] args)
    {
        Log(LogLevel.Debug, args);
    }

    public void Error(params object?[] args)
    {
        Log(LogLevel.Error, args);
    }

    public void Info(params object?[] args)
    {
        Log(LogLevel.Info, args);
    }

    public void Warn(params object?[] args)
    {
        Log(LogLevel.Warn, args);
    }

    public void Log(LogLevel level, params object?[] args)
    {
        Write(level, args);
    }

    public ILogger Create(string name)
    {
        var logger = new ConsoleLogger(name, Level);
        logger._pattern = _pattern;
        return logger;
    }

    public ILogger Child(string name)
    {
        var logger = new ConsoleLogger($"{Name}.{name}", Level);
        logger._pattern = _pattern;
        return logger;
    }

    public ILogger Peer(string name)
    {
        var parts = Name.Split('.').ToList();
        parts.RemoveAt(parts.Count - 1);
        var logger = new ConsoleLogger($"{string.Join('.', parts)}.{name}", Level);
        logger._pattern = _pattern;
        return logger;
    }

    public bool IsEnabled(LogLevel level)
    {
        return level <= Level && _pattern.IsMatch(Name);
    }

    public ILogger SetLevel(LogLevel level)
    {
        Level = level;
        return this;
    }

    public object Clone() => MemberwiseClone();
    public ILogger Copy() => (ILogger)Clone();

    protected void Write(LogLevel level, params object?[] args)
    {
        if (!IsEnabled(level)) return;

        var prefix = $"{level.Color()}{ANSI.Bold.Value}[{level.ToString()?.ToUpper()}]";
        var name = $"{Name}{ANSI.ForegroundReset.Value}{ANSI.BoldReset.Value}";

        foreach (var arg in args)
        {
            var text = arg?.ToString() ?? "null";

            foreach (var line in text.Split('\n'))
            {
                Console.WriteLine("{0} {1} {2}", prefix, name, line);
            }
        }
    }

    protected Regex ParseMagicExpression(string pattern)
    {
        var res = "";
        var parts = pattern.Split('*');

        for (var i = 0; i < parts.Length; i++)
        {
            if (i > 0) res += ".*";
            res += parts[i];
        }

        return new Regex(res);
    }
}