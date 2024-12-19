using BenchmarkDotNet.Attributes;
using InfrastructureExtensions;
using Microsoft.Extensions.Logging;

namespace TestDotNetAndStuff.Logger;

[MemoryDiagnoser]
public class LoggerBenchmarkTest
{
    private const string LogMessageWithParameters = "Logging message {0}, {1} and {2}";
    private const string LogMessage = "Logging message";

    private readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder =>
    {
        builder.AddConsole().SetMinimumLevel(LogLevel.Warning);
    });

    private readonly ILogger<LoggerBenchmarkTest> _logger;
    private readonly ILoggerAdapter<LoggerBenchmarkTest> _loggerAdapter;


    public LoggerBenchmarkTest()
    {
        _logger = new Logger<LoggerBenchmarkTest>(_loggerFactory);
        _loggerAdapter = new LoggerAdapter<LoggerBenchmarkTest>(_logger);
    }

    [Benchmark]
    public void StandardLogger_LogWithoutIf_Parameters()
    {
        _logger.LogInformation(LogMessageWithParameters, 1, 2, 3);
    }

    [Benchmark]
    public void StandardLogger_LogWithAdapter_Parameters()
    {
        _loggerAdapter.LogInformation(LogMessageWithParameters, 1, 2, 3);
    }

    [Benchmark]
    public void StandardLogger_LogWithoutIf_NoParameters()
    {
        _logger.LogInformation(LogMessage);
    }

    [Benchmark]
    public void StandardLogger_LogWithAdapter_NoParameters()
    {
        _loggerAdapter.LogInformation(LogMessage);
    }
}