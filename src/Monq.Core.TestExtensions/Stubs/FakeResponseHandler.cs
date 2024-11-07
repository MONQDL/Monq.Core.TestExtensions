using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Monq.Core.TestExtensions.Stubs
{
    /// <summary>
    /// Фейковый обработчик HTTP запросов.
    /// </summary>
    public class FakeResponseHandler : HttpMessageHandler
    {
        readonly Dictionary<Uri, Func<HttpResponseMessage>> _fakeResponses = new Dictionary<Uri, Func<HttpResponseMessage>>();

        /// <summary>
        /// Добавить ответ <paramref name="responseMessage"/>,
        /// который содержит в себе сообщение <paramref name="content"/> в сериализованном виде,
        /// для заданного маршрута <paramref name="uri"/>.
        /// </summary>
        /// <param name="uri">Маршрут, по которому будет отдан данный ответ.</param>
        /// <param name="responseMessage">Сообщение ответа (код статуса запроса).</param>
        /// <param name="content">Тело сообщения в JSON формате.</param>
        public void AddFakeResponse(Uri uri, HttpResponseMessage responseMessage, string content)
        {
            responseMessage.Content = new StringContent(content);
            _fakeResponses.Add(uri, () => responseMessage);
        }

        /// <summary>
        /// Добавить ответ, который содержит в себе сообщение <paramref name="content"/> в сериализованном виде,
        /// для заданного маршрута <paramref name="uri"/>.
        /// </summary>
        /// <param name="uri">Маршрут, по которому будет отдан данный ответ.</param>
        /// <param name="statusCode">Код статуса запроса.</param>
        /// <param name="content">Тело сообщения в JSON формате.</param>
        public void AddFakeResponse(Uri uri, HttpStatusCode statusCode, string content) => 
            _fakeResponses.Add(uri, () => new HttpResponseMessage() { StatusCode = statusCode, Content = new StringContent(content) });

        /// <inheritdoc />
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_fakeResponses.ContainsKey(request.RequestUri))
                return await Task.FromResult(_fakeResponses[request.RequestUri]());

            return await Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound) { RequestMessage = request });
        }
    }
}