namespace Updater.Core.Abstractions;

public interface ILogger
{
    void Info(string message);
    void Warning(string message);
    void Error(string message, Exception? ex = null);
}

public  enum LogLevel
{
    INFO,
    WARNING,
    ERROR
}