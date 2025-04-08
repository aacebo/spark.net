using Microsoft.Extensions.Logging;
using Microsoft.Spark.Common.Logging;

namespace Microsoft.Spark.AspNetCore;

public static class LoggerFactoryExtensions
{
    public static ILoggerFactory AddSpark(this ILoggerFactory factory, Common.Logging.ILogger? logger = null)
    {
        logger ??= new ConsoleLogger();
        factory.AddProvider(new SparkLoggerProvider(new SparkLogger(logger)));
        return factory;
    }
}