using Updater.Core.Abstractions;

namespace Updater.Core.Services;

public sealed class FileLogger : ILogger
{
    public static event Action<string>? OnLog;
   
    private readonly string path;
    private readonly object sync = new();

    public FileLogger(string _path)
    {
        this.path = _path;
        var dir=Path.GetDirectoryName(path);

        if(!string.IsNullOrWhiteSpace(dir))
            Directory.CreateDirectory(dir);
    }
    public void Info(string message)
        => Write(LogLevel.INFO, message, null);

    public void Warning(string message)
        => Write(LogLevel.WARNING, message,null);
    public void Error(string message, Exception? ex = null)
        =>Write(LogLevel.ERROR,message,ex);

    public void Write(LogLevel level, string message, Exception? ex) 
    {
        if (string.IsNullOrWhiteSpace(message))
            return;
        var line = $"{DateTime.UtcNow:O} [{level}] {message}" +
            (ex == null ? "" : $" | {ex.GetType().Name}:{ex.Message}");

        lock (sync) 
        {
            File.AppendAllText(path, line + Environment.NewLine);
            OnLog?.Invoke(line);
        }
    }
}
