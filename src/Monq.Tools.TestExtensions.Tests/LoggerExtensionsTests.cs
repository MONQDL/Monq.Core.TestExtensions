using Moq;
using Xunit;
using System;
using Microsoft.Extensions.Logging;

namespace Monq.Tools.TestExtensions.Tests
{
    public class LoggerExtensionsTests
    {
        readonly Mock<ILogger<object>> _logger = new Mock<ILogger<object>>();

        [Theory(DisplayName = "Проверка валидации логгера.")]
        [InlineData(int.MaxValue)]
        public void ShouldProperlyVerifyLog(int seed)
        {
            var sporadic = new Random(seed);
            var logMessage = sporadic.GetRandomDescription();

            _logger.Object.LogInformation(logMessage);

            _logger.VerifyLog(LogLevel.Information, logMessage);
        }
    }
}