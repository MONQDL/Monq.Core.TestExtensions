using System;
using System.Linq;
using System.Collections.Generic;

namespace Xunit
{
    /// <summary>
    /// Методы расширения для обобщенных (<see cref="IEnumerable{T}"/>) и необобщенных (<see cref="Array"/>) коллекций и словарей.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Сгенерировать случайный идентификатор для сущности, который не входит в коллекцию.
        /// </summary>
        /// <typeparam name="TModel">Модель сущности.</typeparam>
        /// <param name="entities">Набор существующих сущностей.</param>
        /// <param name="sporadic">ГПСЧ.</param>
        /// <returns></returns>
        public static long GetRandomExceptedId<TModel>(this Dictionary<long, TModel> entities, Random sporadic) where TModel : class
        {
            var newId = sporadic.GetId();

            while (entities.ContainsKey(newId))
                newId = sporadic.GetId();

            return newId;
        }

        /// <summary>
        /// Сгенерировать случайный идентификатор для сущности, который не входит в коллекцию.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">The entities.</param>
        /// <param name="fieldSelector">The field selector.</param>
        /// <param name="sporadic">The sporadic.</param>
        /// <returns></returns>
        public static long GetRandomExceptedId<T>(this IEnumerable<T> entities, Func<T, long> fieldSelector, Random sporadic) where T : class
        {
            var newId = sporadic.GetId();

            var ids = entities.Select(fieldSelector);

            while (ids.Contains(newId))
                newId = sporadic.GetId();

            return newId;
        }

        /// <summary>
        /// Получить случайное количество элементов из коллекции.
        /// </summary>
        /// <param name="entities">Коллекция.</param>
        /// <param name="sporadic">ГПСЧ.</param>
        /// <param name="min">Минимальное кол-во элементов.</param>
        /// <param name="max">Максимальное кол-во элементов.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetRandomItems<T>(this IEnumerable<T> entities, Random sporadic, int min = 1, int? max = null) =>
            entities.Take(sporadic.Next(min, max ?? entities.Count()));

        /// <summary>
        /// Получить случайный элемент из необобщенной коллекции.
        /// </summary>
        /// <typeparam name="T">Тип, в который будет приведен полученный случайный элемент из коллекции.</typeparam>
        /// <param name="array">Необобщенная коллекция.</param>
        /// <param name="sporadic">ГПСЧ.</param>
        /// <param name="allowNull">Может ли вернуться пустой элемент.</param>
        /// <returns></returns>
        public static T GetRandomItem<T>(this Array array, Random sporadic, bool allowNull = false)
        {
            var index = sporadic.Next(allowNull ? -1 : 0, array.Length);
            return index < 0 ? default : (T) array.GetValue(index);
        }

        /// <summary>
        /// Получить случайный элемент из коллекции.
        /// </summary>
        /// <typeparam name="T">Тип элемента обобщенной коллекции.</typeparam>
        /// <param name="entities">Коллекция.</param>
        /// <param name="sporadic">ГПСЧ.</param>
        /// <param name="allowNull">Может ли вернуться пустой элемент.</param>
        /// <returns></returns>
        public static T GetRandomItem<T>(this IEnumerable<T> entities, Random sporadic, bool allowNull = false)
        {
            int index = sporadic.Next(allowNull ? -1 : 0, entities.Count());
            return allowNull ? entities.ElementAtOrDefault(index) : entities.ElementAt(index);
        }

        /// <summary>
        /// Перевод коллекции из ulong в long.
        /// </summary>
        /// <param name="items">Коллекция элементов из ulong.</param>
        /// <returns>Коллекция элементов из long.</returns>
        public static IEnumerable<long> ConvertToSigned(this IEnumerable<ulong> items)
        {
            foreach (var item in items)
                yield return (long)item;
        }
    }
}