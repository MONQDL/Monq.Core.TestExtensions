using System;
using System.Linq;
using System.Collections.Generic;

namespace Xunit
{
    /// <summary>
    /// Методы расширения для обобщенных (<see cref="IEnumerable{T}"/>)
    /// и необобщенных (<see cref="Array"/>) коллекций и словарей.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Получить коллекцию всех значений перечисления.
        /// </summary>
        /// <typeparam name="T">Тип перечисления.</typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetValues<T>() where T : Enum 
            => Enum.GetValues(typeof(T)).Cast<T>();

        /// <summary>
        /// Сгенерировать случайный идентификатор для сущности, который не входит в словарь локальной БД.
        /// </summary>
        /// <typeparam name="TModel">Модель сущности.</typeparam>
        /// <param name="entities">Набор существующих сущностей.</param>
        /// <param name="sporadic">ГПСЧ.</param>
        /// <param name="minValue">Минимальное возможное значение.</param>
        /// <returns></returns>
        public static long GetRandomExceptedId<TModel>(this Dictionary<long, TModel> entities,
            Random sporadic,
            in long minValue = 1)
            where TModel : class
        {
            var newId = sporadic.GetId(minValue);

            while (entities.ContainsKey(newId))
                newId = sporadic.GetId(minValue);

            return newId;
        }

        /// <summary>
        /// Сгенерировать случайный идентификатор для сущности,
        /// который не входит в коллекцию <paramref name="entities"/> по заданному селектору <paramref name="propSelector"/>.
        /// </summary>
        /// <typeparam name="T">Тип элемента коллекции.</typeparam>
        /// <param name="entities">Коллекция элементов.</param>
        /// <param name="sporadic">ГПСЧ.</param>
        /// <param name="propSelector">Селектор поля модели <typeparamref name="T"/>,
        /// по которому будет происходить сравнение сгенерированного идентификатора.</param>
        /// <param name="minValue">Минимальное возможное значение.</param>
        /// <returns></returns>
        public static long GetRandomExceptedId<T>(this IEnumerable<T> entities,
            Random sporadic,
            Func<T, long> propSelector,
            in long minValue = 1)
            where T : class
        {
            var ids = entities.Select(propSelector);
            return GetRandomExceptedId(ids, sporadic, minValue);
        }

        /// <summary>
        /// Сгенерировать случайный идентификатор, который не входит в коллекцию.
        /// </summary>
        /// <param name="items">Коллекция идентификаторов.</param>
        /// <param name="sporadic">ГПСЧ.</param>
        /// <param name="minValue">Минимальное возможное значение.</param>
        /// <returns></returns>
        public static long GetRandomExceptedId(this IEnumerable<long> items, Random sporadic, in long minValue = 1)
        {
            var newId = sporadic.GetId(minValue);

            while (items.Contains(newId))
                newId = sporadic.GetId(minValue);

            return newId;
        }

        /// <summary>
        /// Сгенерировать случайное наименование,
        /// которое не входит в коллекцию <paramref name="entities"/> по заданному селектору <paramref name="propSelector"/>.
        /// </summary>
        /// <typeparam name="T">Тип элемента коллекции.</typeparam>
        /// <param name="entities">Коллекция элементов.</param>
        /// <param name="sporadic">ГПСЧ.</param>
        /// <param name="propSelector">Селектор поля модели <typeparamref name="T"/>,
        /// по которому будет происходить сравнение сгенерированного наименования.</param>
        /// <param name="comparer">Компаратор строк (по умолчанию = Ordinal).</param>
        /// <param name="minLength">Мин. длина строки.</param>
        /// <param name="maxLength">Макс. длина строки.</param>
        /// <returns></returns>
        public static string GetRandomExceptedName<T>(this Dictionary<long, T> entities,
            Random sporadic,
            Func<T, string> propSelector,
            StringComparer comparer = null,
            in int minLength = 4,
            in int maxLength = 16)
            where T : class
        {
            var newName = sporadic.GetRandomName(minLength, maxLength);

            var names = entities.Values.Select(propSelector);

            while (names.Contains(newName, comparer ?? StringComparer.Ordinal))
                newName = sporadic.GetRandomName(minLength, maxLength);

            return newName;
        }

        /// <summary>
        /// Сгенерировать случайное наименование,
        /// которое не входит в коллекцию <paramref name="entities"/> по заданному селектору <paramref name="propSelector"/>.
        /// </summary>
        /// <typeparam name="T">Тип элемента коллекции.</typeparam>
        /// <param name="entities">Коллекция элементов.</param>
        /// <param name="sporadic">ГПСЧ.</param>
        /// <param name="propSelector">Селектор поля модели <typeparamref name="T"/>,
        /// по которому будет происходить сравнение сгенерированного наименования.</param>
        /// <param name="comparer">Компаратор строк (по умолчанию = Ordinal).</param>
        /// <param name="minLength">Мин. длина строки.</param>
        /// <param name="maxLength">Макс. длина строки.</param>
        /// <returns></returns>
        public static string GetRandomExceptedName<T>(this IEnumerable<T> entities,
            Random sporadic,
            Func<T, string> propSelector,
            StringComparer comparer = null,
            in int minLength = 4,
            in int maxLength = 16)
            where T : class
        {
            var newName = sporadic.GetRandomName(minLength, maxLength);

            var names = entities.Select(propSelector);

            while (names.Contains(newName, comparer ?? StringComparer.Ordinal))
                newName = sporadic.GetRandomName(minLength, maxLength);

            return newName;
        }

        /// <summary>
        /// Получить случайное количество элементов из коллекции.
        /// </summary>
        /// <param name="entities">Коллекция.</param>
        /// <param name="sporadic">ГПСЧ.</param>
        /// <param name="min">Минимальное кол-во элементов.</param>
        /// <param name="max">Максимальное кол-во элементов.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetRandomItems<T>(this IEnumerable<T> entities,
            Random sporadic,
            int min = 1,
            int? max = null) =>
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