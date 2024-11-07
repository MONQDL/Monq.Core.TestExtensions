using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Monq.Core.TestExtensions
{
    /// <summary>
    /// Методы расширения для контроллера ASP.NET Core MVC <see cref="Controller"/>.
    /// </summary>
    public static class MvcExtensions
    {
        /// <summary>
        /// Задать идентификатор пользовательского пространства в http контекст контроллера.
        /// </summary>
        /// <param name="controller">Web.Api контроллер.</param>
        /// <param name="userspaceId">Идентификатор пользовательского пространства.</param>
        /// <param name="fieldName">Название поля, содерждащего значение userspaceId в контроллере.</param>
        public static void SetUserspace(this Controller controller, long userspaceId, string fieldName = "UserspaceId")
        {
            var userspaceField = controller.GetType()
                .GetField(fieldName,
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            userspaceField.SetValue(controller, userspaceId);
        }

        /// <summary>
        /// Создать мета-данные по запросам, которые будут приходить в MVC-контроллер во время выполнения unit-тестов.
        /// </summary>
        /// <param name="httpRequest">HTTP-контекст контроллера, на который будут поступать запросы.</param>
        /// <param name="path">Базовый url.</param>
        /// <param name="scheme">Протокол.</param>
        /// <param name="host">Адрес хоста.</param>
        /// <param name="port">Порт.</param>
        /// <returns></returns>
        public static void CreateTestHttpRequestMetaInfo(this HttpRequest httpRequest, string path,
            string scheme = "http",
            string host = "localhost",
            int port = 5005)
        {
            httpRequest.Scheme = scheme;
            httpRequest.Host = new HostString(host, port);
            httpRequest.Path = path;
        }

        /// <summary>
        /// Добавить "x-smon-userspace-id" в заголовки HTTP запроса/ответа.
        /// </summary>
        /// <param name="headers">Заголовки HTTP запроса/ответа.</param>
        /// <param name="userspaceId">Идентификатор пользовательского пространства.</param>
        public static void AddUserspaceId(this IHeaderDictionary headers, long userspaceId) =>
            headers.Add("x-smon-userspace-id", new StringValues(userspaceId.ToString()));
    }
}