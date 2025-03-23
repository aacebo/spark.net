
using System.Text.RegularExpressions;

namespace Microsoft.Spark.Common.Logging;

public partial class ConsoleLogger<T>(LogLevel level = LogLevel.Info) : ConsoleLogger(typeof(T).Name, level), ILogger<T>;
public partial class ConsoleLogger : ILogger
{
    public string Name { get; }
    public LogLevel Level { get; set; }

    protected Regex _pattern;

    public ConsoleLogger(string name, LogLevel level = LogLevel.Info)
    {
        Name = name;
        Level = level;
        _pattern = ParseMagicExpression(Environment.GetEnvironmentVariable("LOG") ?? "*");
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

    public ILogger Child(string name)
    {
        return new ConsoleLogger($"{Name}.{name}", Level);
    }

    public bool IsEnabled(LogLevel level)
    {
        return level <= Level || !_pattern.IsMatch(Name);
    }

    public ILogger SetLevel(LogLevel level)
    {
        Level = level;
        return this;
    }

    internal ANSI GetLevelColor(LogLevel level)
    {
        return level == LogLevel.Error ? ANSI.ForegroundRed
             : level == LogLevel.Warn ? ANSI.ForegroundYellow
             : level == LogLevel.Info ? ANSI.ForegroundCyan
             : ANSI.ForegroundMagenta;
    }

    protected void Write(LogLevel level, params object?[] args)
    {
        if (!IsEnabled(level)) return;

        var prefix = $"{GetLevelColor(level)}{ANSI.Bold.Value}[{Enum.GetName(level)?.ToUpper()}]";
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