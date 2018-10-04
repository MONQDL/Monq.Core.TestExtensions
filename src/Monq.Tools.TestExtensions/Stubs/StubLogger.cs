using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Monq.Tools.TestExtensions.Stubs
{
#pragma warning disable CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена
#pragma warning disable RCS1163 // Unused parameter.
#pragma warning disable RECS0154 // Parameter is never used
#pragma warning disable S1186 // Methods should not be empty

    public class StubLogger : ILogger
    {
        public List<string> LoggingEvents { get; set; } = new List<string>();

        public void Log(LogLevel logLevel, int eventId, object state, Exception exception, Func<object, Exception, string> formatter) => LoggingEvents.Add(state.ToString());

        public bool IsEnabled(LogLevel logLevel) => false;

        public IDisposable BeginScopeImpl(object state) => null;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) => LoggingEvents.Add(state.ToString());

        public IDisposable BeginScope<TState>(TState state) => null;
    }

    public class StubLogger<T> : StubLogger, ILogger<T> { }

#pragma warning disable CA1063 // Implement IDisposable Correctly
#pragma warning disable S3881 // "IDisposable" should be implemented correctly

    public class StubLoggerFactory : ILoggerFactory
    {
        readonly IList<StubLogger> _loggers = new List<StubLogger>();

        public StubLoggerFactory(IList<StubLogger> loggers) => _loggers = loggers;

        public StubLoggerFactory()
        { }

        public void AddProvider(ILoggerProvider provider) => throw new NotImplementedException();

        public ILogger<T> CreateLog<T>()
        {
            var logger = new StubLogger<T>();
            _loggers.Add(logger);
            return logger;
        }

        public ILogger CreateLogger(string categoryName)
        {
            var logger = new StubLogger();
            _loggers.Add(logger);
            return logger;
        }

        public void Dispose()
        {
        }
    }
}