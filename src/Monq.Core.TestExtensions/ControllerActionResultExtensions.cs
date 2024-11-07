using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monq.Core.MvcExtensions.ViewModels;

namespace Monq.Core.TestExtensions
{
    /// <summary>
    /// Методы расширения для тестирования результата действия контроллера <see cref="ActionResult{TValue}"/>.
    /// </summary>
    public static class ControllerActionResultExtensions
    {
        /// <summary>
        /// Верификация действия контроллера, которое возвращает коллекцию типа <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">Тип элемента коллекции.</typeparam>
        /// <param name="listResult">Результат действия контроллера.</param>
        /// <returns>
        /// Коллекция, которая была включена в результат действия.
        /// </returns>
        public static IList<T> VerifyAction<T>(this ActionResult<IList<T>> listResult) where T : class
        {
            Assert.Null(listResult.Result);
            Assert.NotEmpty(listResult.Value);

            return listResult.Value;
        }

        /// <summary>
        /// Верификация действия контроллера, которое возвращает объект типа <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого объекта.</typeparam>
        /// <param name="objectResult">Результат действия контроллера.</param>
        /// <returns>
        /// Объект, который был включен в результат действия.
        /// </returns>
        public static T VerifyAction<T>(this ActionResult<T> objectResult) where T : class
        {
            Assert.Null(objectResult.Result);
            Assert.NotNull(objectResult.Value);

            return objectResult.Value;
        }

        /// <summary>
        /// Верификация действия контроллера, которое возвращает <see cref="CreatedAtActionResult"/> с вложенным объектом <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Тип вложенного объекта.</typeparam>
        /// <param name="objectResult">Результат действия контроллера.</param>
        /// <returns>
        /// Объект, который был включен в результат действия.
        /// </returns>
        public static T VerifyCreatedAtAction<T>(this ActionResult<T> objectResult) where T : class
        {
            var createdResult = Assert.IsAssignableFrom<CreatedAtActionResult>(objectResult.Result);
            var result = Assert.IsAssignableFrom<T>(createdResult?.Value);
            Assert.NotNull(result);
            return result;
        }

        /// <summary>
        /// Верификация действия контроллера, которое возвращает <see cref="BadRequestObjectResult"/>.
        /// </summary>
        /// <param name="actionResult">Результат действия контроллера.</param>
        /// <param name="expectedMessage">Ожидаемое сообщение об ошибке.</param>
        public static void VerifyBadRequest<T>(this ActionResult<T> actionResult, string expectedMessage)
        {
            var result = actionResult.Result;
            Assert.NotNull(result);
            Assert.Null(actionResult.Value);
            VerifyBadRequest(result, expectedMessage);
        }

        /// <summary>
        /// Верификация действия контроллера, которое возвращает <see cref="BadRequestObjectResult"/>.
        /// </summary>
        /// <param name="actionResult">Результат действия контроллера.</param>
        /// <param name="expectedMessage">Ожидаемое сообщение об ошибке.</param>
        public static void VerifyBadRequest(this ActionResult actionResult, string expectedMessage)
        {
            var result = Assert.IsAssignableFrom<BadRequestObjectResult>(actionResult);
            var errorResponse = Assert.IsAssignableFrom<ErrorResponseViewModel>(result.Value);
            Assert.Equal(expectedMessage, errorResponse.Message);
        }

        /// <summary>
        /// Верификация действия контроллера, которое возвращает <see cref="StatusCodes.Status404NotFound"/> в случае отсутствия искомой сущности.
        /// </summary>
        /// <param name="actionResult">Результат действия контроллера.</param>
        /// <param name="expectedMessage">Ожидаемое сообщение об ошибке.</param>
        public static void VerifyNotFoundRequest<T>(this ActionResult<T> actionResult, string expectedMessage)
        {
            var result = actionResult.Result;
            Assert.NotNull(result);
            Assert.Null(actionResult.Value);
            VerifyNotFoundRequest(result, expectedMessage);
        }

        /// <summary>
        /// Верификация действия контроллера, которое возвращает <see cref="StatusCodes.Status404NotFound"/> в случае отсутствия искомой сущности.
        /// </summary>
        /// <param name="actionResult">Результат действия контроллера.</param>
        /// <param name="expectedMessage">Ожидаемое сообщение об ошибке.</param>
        public static void VerifyNotFoundRequest(this ActionResult actionResult, string expectedMessage)
        {
            var result = Assert.IsAssignableFrom<NotFoundObjectResult>(actionResult);
            var errorResponse = Assert.IsAssignableFrom<ErrorResponseViewModel>(result.Value);
            Assert.Equal(expectedMessage, errorResponse.Message);
        }

        /// <summary>
        /// Верификация действия контроллера, которое возвращает <see cref="StatusCodes.Status403Forbidden"/> в случае отсутствия прав на выполнение данной операции.
        /// </summary>
        /// <param name="actionResult">Результат действия контроллера.</param>
        /// <param name="expectedMessage">Ожидаемое сообщение об ошибке.</param>
        public static void VerifyForbiddenRequest<T>(this ActionResult<T> actionResult, string expectedMessage)
        {
            var result = actionResult.Result;
            Assert.NotNull(result);
            Assert.Null(actionResult.Value);
            VerifyForbiddenRequest(result, expectedMessage);
        }

        /// <summary>
        /// Верификация действия контроллера, которое возвращает <see cref="StatusCodes.Status403Forbidden"/> в случае отсутствия прав на выполнение данной операции.
        /// </summary>
        /// <param name="actionResult">Результат действия контроллера.</param>
        /// <param name="expectedMessage">Ожидаемое сообщение об ошибке.</param>
        public static void VerifyForbiddenRequest(this ActionResult actionResult, string expectedMessage)
        {
            var result = Assert.IsAssignableFrom<ObjectResult>(actionResult);
            Assert.Equal(StatusCodes.Status403Forbidden, result.StatusCode);
            var errorResponse = Assert.IsAssignableFrom<ErrorResponseViewModel>(result.Value);
            Assert.Equal(expectedMessage, errorResponse.Message);
        }
    }
}