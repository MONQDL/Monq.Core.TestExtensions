using Monq.Core.Paging.Models;

namespace Monq.Core.TestExtensions
{
    /// <summary>
    /// Класс содержит набор констант для проекта с unit-тестами.
    /// </summary>
    public static class TestConstants
    {
        /// <summary>
        /// Экземпляр модели постраничной навигации для получения всех записей.
        /// </summary>
        public static PagingModel AllRecords => new PagingModel
        {
            PerPage = PagingModel.ALL_ITEMS_PER_PAGE
        };
    }
}