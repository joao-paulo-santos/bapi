namespace Infrastructure.Logging
{
    public class SerilogLogger : Core.Interfaces.ILogger
    {
        private readonly Serilog.ILogger _logger;
        public SerilogLogger(Serilog.ILogger logger)
        {
            _logger = logger;
        }
        public void LogDebug(string message)
        {
            _logger.Debug(message);
        }

        public void LogError(string message, Exception? exception = null)
        {
            _logger.Error(message, exception);
        }

        public void LogInformation(string message)
        {
            _logger.Information(message);
        }

        public void LogWarning(string message)
        {
            _logger.Warning(message);
        }

    }
}
