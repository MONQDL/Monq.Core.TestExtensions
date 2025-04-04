using Microsoft.Extensions.Logging;
using Moq;

namespace Xunit;

/// <summary>
/// Методы расширения для работы с <see cref="Mock{ILogger}"/>.
/// </summary>
public static class LoggerExtensions
{
    /// <summary>
    /// Верифицировать логгер по указанному сообщению, которое наступило однократно.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="loggerMock">The logger mock.</param>
    /// <param name="level">Уровень сообщения.</param>
    /// <param name="message">Содержимое сообщения.</param>
    /// <param name="failMessage">Сообщение при наступлении исключения.</param>
    public static void VerifyLog<T>(this Mock<ILogger<T>> loggerMock, LogLevel level, string message, string failMessage = null) =>
        loggerMock.VerifyLog(level, message, Times.Once(), failMessage);

    /// <summary>
    /// Верифицировать логгер по указанному сообщению, которое наступило указанное количество раз.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="loggerMock">The logger mock.</param>
    /// <param name="level">Уровень сообщения.</param>
    /// <param name="message">Содержимое сообщения.</param>
    /// <param name="times">Количество поступивших сообщений.</param>
    /// <param name="failMessage">Сообщение при наступлении исключения.</param>
    public static void VerifyLog<T>(this Mock<ILogger<T>> loggerMock, LogLevel level, string message, Times times, string failMessage = null) =>
        loggerMock.Verify(l => l.Log(level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString() == message),
                null,
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            times,
            failMessage);

    /// <summary>
    /// Верифицировать логгер по указанному исключению с сообщением.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TException">Тип сгенерированного исключения, которое зафиксировал логгер.</typeparam>
    /// <param name="loggerMock">The logger mock.</param>
    /// <param name="level">Уровень сообщения.</param>
    /// <param name="message">Содержимое сообщения.</param>
    public static void VerifyLog<T, TException>(this Mock<ILogger<T>> loggerMock, LogLevel level, string message) where TException : Exception =>
        loggerMock.Verify(obj =>
                obj.Log(
                    level,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString() == message),
                    It.IsAny<TException>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
}
