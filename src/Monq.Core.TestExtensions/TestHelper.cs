using System;
using System.Net.Http;
using IdentityModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Monq.Core.HttpClientExtensions;
using Monq.Core.TestExtensions.Stubs;
using Monq.Core.BasicDotNetMicroservice;

namespace Monq.Core.TestExtensions
{
    /// <summary>
    /// Хелпер для создания окружения unit-тестов.
    /// </summary>
    public static class TestHelper
    {
        /// <summary>
        /// Создать пользователя, по которому будет производиться проверка прав в действиях контроллера,
        /// а также производиться логгирование изменений по сущностям через DbModelTracking.
        /// </summary>
        /// <param name="subjectId">Идентификатор пользователя.</param>
        /// <param name="userName">Имя Пользователя.</param>
        /// <returns></returns>
        public static ClaimsPrincipal CreateUser(in long subjectId, string userName = null)
        {
            var name = !string.IsNullOrWhiteSpace(userName)
                ? userName
                : (subjectId == -1 ? "Системный пользователь" : "Тестовый пользователь");

            var claims = new[]
            {
                new Claim(JwtClaimTypes.Subject, subjectId.ToString()),
                new Claim(JwtClaimTypes.Name, name)
            };

            var ci = new ClaimsIdentity(claims, string.Empty, JwtClaimTypes.Name, JwtClaimTypes.Role);
            return new ClaimsPrincipal(ci);
        }

        /// <summary>
        /// Создать экземпляр реализации http сервиса, которая работает через <see cref="FakeResponseHandler"/>.
        /// </summary>
        /// <typeparam name="TInt">Интерфейс HTTP-сервиса.</typeparam>
        /// <typeparam name="TImpl">Реализация интерфейса HTTP-сервиса.</typeparam>
        /// <param name="appConfiguration">Экземпляр параметров микросервиса, которые включают в себя базовый Uri, относительно которого строятся запросы к Api СМ.</param>
        /// <param name="fakeResponseHandler">Фейковый обработчик http-запросов, которые посылает сервис.</param>
        /// <param name="context">HTTP контекст.</param>
        /// <returns></returns>
        public static TInt CreateHttpService<TInt, TImpl>(
            IOptions<AppConfiguration> appConfiguration,
            FakeResponseHandler fakeResponseHandler,
            HttpContextAccessor context = null)
            where TImpl : RestHttpClientFromOptions<AppConfiguration>, TInt
        {
            context ??= new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext()
            };
            
            return (TImpl)Activator.CreateInstance(typeof(TImpl),
                appConfiguration,
                new HttpClient(fakeResponseHandler),
                new LoggerFactory(),
                new RestHttpClientOptions(),
                context);
        }
    }
}