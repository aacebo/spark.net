using Microsoft.Extensions.Logging;
using Microsoft.Spark.Common.Logging;

namespace Microsoft.Spark.Extensions.Logging;

public static class LoggerFactoryExtensions
{
    public static ILoggerFactory AddSpark(this ILoggerFactory factory, Common.Logging.ILogger? logger = null)
    {
        factory.AddProvider(new SparkLoggerProvider(new SparkLogger(logger ?? new ConsoleLogger())));
        return factory;
    }
}