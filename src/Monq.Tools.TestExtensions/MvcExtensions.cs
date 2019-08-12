using Microsoft.AspNetCore.Mvc;

namespace Xunit
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
        /// <param name="userspaceId">Идентификатор пользовательского пространства</param>
        public static void SetUserspace(this Controller controller, long userspaceId)
        {
            var userspaceField = controller.GetType()
                .GetField("UserspaceId",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            userspaceField.SetValue(controller, userspaceId);
        }
    }
}